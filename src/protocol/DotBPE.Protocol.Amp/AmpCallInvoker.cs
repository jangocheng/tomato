﻿using System;
using System.Threading.Tasks;
using DotBPE.Rpc;
using System.Collections.Concurrent;
using DotBPE.Rpc.Logging;
using DotBPE.Rpc.Exceptions;

namespace DotBPE.Protocol.Amp
{
    public class AmpCallInvoker : CallInvoker<AmpMessage>
    {
        static readonly ILogger Logger = DotBPE.Rpc.Environment.Logger.ForType<AmpMessage>();

        private readonly ConcurrentDictionary<string, TaskCompletionSource<AmpMessage>> _resultDictionary =
new ConcurrentDictionary<string, TaskCompletionSource<AmpMessage>>();

        private int sendSequence = 0 ;
        private static object lockObj = new object();
        public AmpCallInvoker(IRpcClient<AmpMessage> rpcClient) : base(rpcClient)
        {
        }
            
        public async override Task<AmpMessage> AsyncCall(AmpMessage request, int timeOut = 3000)
        {
            try
            {
                AutoSetSequence(request);
                var callbackTask = RegisterResultCallbackAsync(request.Id,timeOut);
                          
                try
                {                   
                    //发送                  
                    await base.RpcClient.SendAsync(request);
                }
                catch (Exception exception)
                {
                    RemoveResultCallback(request.Id);
                    throw new RpcCommunicationException ("与服务端通讯时发生了异常。", exception);
                }
                
                return await callbackTask;
            }
            catch (Exception exception)
            {             
                Logger.Error("消息发送失败。", exception);
                throw;
            }
        }

       
        public override AmpMessage BlockingCall(AmpMessage request)
        {
            return this.AsyncCall(request).Result;           
        }

        protected override void MessageRecieved(object sender, MessageRecievedEventArgs<AmpMessage> e)
        {
            if(e.Message == null)
            {
                throw new RpcBizException("收到了空消息");
            }

            if(e.Message.InvokeMessageType == Rpc.Codes.InvokeMessageType.Response) //只处理回复消息
            {                
                Logger.Info($"接收到消息:{e.Message.Id}");
                var message = e.Message;
                TaskCompletionSource<AmpMessage> task;
                if (_resultDictionary.ContainsKey(message.Id) 
                    && _resultDictionary.TryGetValue(message.Id, out task))
                {
                    task.SetResult(message);
                    // 移除字典
                    _resultDictionary.TryRemove(message.Id, out var _);
                }
            }
        }


        private void RemoveResultCallback(string id)
        {
            _resultDictionary.TryRemove(id, out var _);
        }

        private void TimeOutCallBack(string id)
        {
            TaskCompletionSource<AmpMessage> task;
            if (_resultDictionary.TryGetValue(id, out task))
            {
                task.SetException(new RpcCommunicationException("操作超时！"));
                // 移除字典
                _resultDictionary.TryRemove(id, out var _);
            }
        }
        private Task<AmpMessage> RegisterResultCallbackAsync(string id,int timeOut)
        {
            var tcs = new TaskCompletionSource<AmpMessage>();
           
            _resultDictionary.TryAdd(id, tcs);
            var task = tcs.Task;
            //设置超时              
            if (Task.WhenAny(task, Task.Delay(timeOut)).Result != task)
            {
                // timeout logic
                TimeOutCallBack(id);
            }
            return task;
        }

        private void AutoSetSequence(AmpMessage request)
        {
            lock (lockObj)
            {
                request.Sequence = this.sendSequence++;
            }           
        }
    }
}