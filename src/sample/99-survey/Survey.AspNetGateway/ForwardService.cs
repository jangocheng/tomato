using System;
using System.Collections.Generic;
using System.Text;
using DotBPE.Plugin.AspNetGateway;
using DotBPE.Protocol.Amp;
using DotBPE.Rpc;
using DotBPE.Rpc.Logging;
using Google.Protobuf;
using Microsoft.Extensions.Options;
using Survey.Core;


namespace Survey.AspNetGateway
{
    /// <summary>
    /// 转发服务，需要根据自己的协议 将HTTP请求的数据转码成对应的协议二进制
    /// 并将协议二进制数据转换成对应的 Http响应数据（一般是json）
    /// </summary>
    public class ForwardService : AbstractForwardService<AmpMessage>
    {

        static readonly JsonFormatter AmpJsonFormatter = new JsonFormatter(new JsonFormatter.Settings(true).WithFormatEnumsAsIntegers(true));

        static ILogger Logger = DotBPE.Rpc.Environment.Logger.ForType<ForwardService>();


        public ForwardService(IRpcClient<AmpMessage> rpcClient,
           IOptionsSnapshot<HttpRouterOption> optionsAccessor) : base(rpcClient, optionsAccessor)
        {
        }

        /// <summary>
        ///  将收集的请求数据转换层协议消息
        /// </summary>
        /// <param name="reqData">请求数据，从body,form和queryString中获取</param>
        /// <returns></returns>
        protected override AmpMessage EncodeRequest(RequestData reqData)
        {
            ushort serviceId = (ushort)reqData.ServiceId;
            ushort messageId = (ushort)reqData.MessageId;
            AmpMessage message = AmpMessage.CreateRequestMessage(serviceId, messageId);


            IMessage reqTemp = ProtobufObjectFactory.GetRequestTemplate(serviceId, messageId);
            if (reqTemp == null)
            {
                Logger.Error("serviceId={0},messageId={1}的消息不存在", serviceId, messageId);
                return null;
            }

            try
            {
                var descriptor = reqTemp.Descriptor;
                if (!string.IsNullOrEmpty(reqData.RawBody))
                {
                    reqTemp = descriptor.Parser.ParseJson(reqData.RawBody);
                }

                if (reqData.Data.Count > 0)
                {
                    foreach (var field in descriptor.Fields.InDeclarationOrder())
                    {
                        if (reqData.Data.ContainsKey(field.Name))
                        {
                            field.Accessor.SetValue(reqTemp, reqData.Data[field.Name]);
                        }
                        else if (reqData.Data.ContainsKey(field.JsonName))
                        {
                            field.Accessor.SetValue(reqTemp, reqData.Data[field.JsonName]);
                        }

                    }
                }


                message.Data = reqTemp.ToByteArray();

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "从HTTP请求中解析数据错误:" + ex.Message);
                message = null;
            }

            return message;

        }
        /// <summary>
        /// 返回协议调用者
        /// </summary>
        /// <param name="rpcClient"></param>
        /// <returns></returns>
        protected override CallInvoker<AmpMessage> GetProtocolCallInvoker(IRpcClient<AmpMessage> rpcClient)
        {
            return new AmpCallInvoker(rpcClient);
        }

        /// <summary>
        /// 从消息中读取SessionId ，只有在配置了需要保持状态的网关上才会执行，并且只有在没有sessionId的时候才读取
        /// </summary>
        /// <param name="message">返回到客户端解析前的消息</param>
        /// <returns></returns>
        protected override string GetSessionIdFromMessage(AmpMessage message)
        {
            if (message.Code == 0)
            {

                if (message != null)
                {
                    var rspTemp = ProtobufObjectFactory.GetResponseTemplate(message.ServiceId, message.MessageId);
                    if (rspTemp == null)
                    {
                        return base.GetSessionIdFromMessage(message);
                    }

                    if (message.Data != null)
                    {
                        rspTemp.MergeFrom(message.Data);
                    }
                    //提取内部的return_code 字段
                    var field_sessionId = rspTemp.Descriptor.FindFieldByName(Constants.SEESIONID_FIELD_NAME);
                    if (field_sessionId != null)
                    {
                        var ObjV = field_sessionId.Accessor.GetValue(rspTemp);
                        if (ObjV != null)
                        {
                            return ObjV.ToString();
                        }
                    }

                }
            }
            return base.GetSessionIdFromMessage(message);
        }


        /// <summary>
        /// 将消息序列化成JSON，可以使用自己喜欢的序列化组件
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected override string MessageToJson(AmpMessage message)
        {
            if (message.Code == 0)
            {
                var return_code = 0;
                var return_message = "";
                string ret = "";
                if (message != null)
                {
                    var rspTemp = ProtobufObjectFactory.GetResponseTemplate(message.ServiceId, message.MessageId);
                    if (rspTemp == null)
                    {
                        return ret;
                    }

                    if (message.Data != null)
                    {
                        rspTemp.MergeFrom(message.Data);
                    }
                   
                    //提取return_message
                    var field_msg = rspTemp.Descriptor.FindFieldByName("return_message");
                    if (field_msg != null)
                    {
                        var retObjV = field_msg.Accessor.GetValue(rspTemp);
                        if (retObjV != null)
                        {
                            return_message = retObjV.ToString();
                        }
                    }

                    ret = AmpJsonFormatter.Format(rspTemp);

                    //TODO:清理内部的return_message ;
                }

                return "{\"return_code\":" + return_code + ",\"return_message\":\"" + return_message + "\",data:" + ret + "}";
            }
            else
            {
                return "{\"return_code\":" + message.Code + ",\"return_message\":\"\"}";
            }


        }
    }
}
