
//---------------------------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated by template for T4.
//    Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
//---------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;

using Vulcan.DataAccess.ORMapping;

namespace Survey.Service.InnerImpl.Domain
{

	[TableName("answer")]
	public partial class Answer : Survey.Service.InnerImpl.Domain.BaseEntity
	{
		private string _AnswerId;
        private int _ApaperId;

        private int _ObjectiveAnswer;

        private string _QuestionId;

        private string _SubjectiveAnswer;

        /// <summary>
        /// 答案ID主键
        ///  varchar(50)
        /// </summary>
        [MapField("answer_id"),   PrimaryKey(1)] 
		public string AnswerId
		{ get{ return _AnswerId; } 	set{ _AnswerId = value ;  OnPropertyChanged("answer_id"); } }
		/// <summary>
		/// 答卷表ID
		///  int(10)
		/// </summary>
		[MapField("apaper_id")    ] 
		public int ApaperId
		{ get{ return _ApaperId; } 	set{ _ApaperId = value ;  OnPropertyChanged("apaper_id"); } }
        /// <summary>
        /// 客观题 答案以二进制方式存放（如果 1 标识 选择了 A，11 选择了AB，101 这选择AC）
        ///  int(10)
        /// </summary>
        [MapField("objective_answer")]
        public int ObjectiveAnswer
        { get { return _ObjectiveAnswer; } set { _ObjectiveAnswer = value; OnPropertyChanged("objective_answer"); } }

        /// <summary>
        /// 
        ///  varchar(50)
        /// </summary>
        [MapField("question_id")    ] 
		public string QuestionId
		{ get{ return _QuestionId; } 	set{ _QuestionId = value ;  OnPropertyChanged("question_id"); } }
		/// <summary>
		/// 主观题答案
		///  varchar(500)
		/// </summary>
		[MapField("subjective_answer"), Nullable  ] 
		public string SubjectiveAnswer
		{ get{ return _SubjectiveAnswer; } 	set{ _SubjectiveAnswer = value ;  OnPropertyChanged("subjective_answer"); } }
	}

	[TableName("apaper")]
	public partial class APaper : Survey.Service.InnerImpl.Domain.BaseEntity
	{
        private DateTime _CreateTime;
        private int _PaperId;
        private int _QpaperId;

        private string _Remark;

        private string _UserId;

        private string _QpaperSubject;

        private string _QpaperUserId;

        /// <summary>
        /// 答题时间
        ///  datetime
        /// </summary>
        [MapField("create_time")]
        public DateTime CreateTime
        { get { return _CreateTime; } set { _CreateTime = value; OnPropertyChanged("create_time"); } }

        /// <summary>
        /// 答卷ID 自增
        ///  int(10)
        /// </summary>
        [MapField("paper_id"), Identity, PrimaryKey(1)] 
		public int PaperId
		{ get{ return _PaperId; } 	set{ _PaperId = value ; } }
		/// <summary>
		/// 
		///  int(10)
		/// </summary>
		[MapField("qpaper_id")    ] 
		public int QpaperId
		{ get{ return _QpaperId; } 	set{ _QpaperId = value ;  OnPropertyChanged("qpaper_id"); } }


        [MapField("qpaper_subject")]
        public string QpaperSubject
        { get { return _QpaperSubject; } set { _QpaperSubject = value; OnPropertyChanged("qpaper_subject"); } }


        [MapField("qpaper_user_id")]
        public string QpaperUserId
        { get { return _QpaperUserId; } set { _QpaperUserId = value; OnPropertyChanged("qpaper_user_id"); } }


        /// <summary>
        /// 备注
        ///  varchar(1000)
        /// </summary>
        [MapField("remark"), Nullable]
        public string Remark
        { get { return _Remark; } set { _Remark = value; OnPropertyChanged("remark"); } }

        /// <summary>
        /// 答题人标识
        ///  varchar(50)
        /// </summary>
        [MapField("user_id")    ] 
		public string UserId
		{ get{ return _UserId; } 	set{ _UserId = value ;  OnPropertyChanged("user_id"); } }

    }

	[TableName("qpaper")]
	public partial class QPaper : Survey.Service.InnerImpl.Domain.BaseEntity
	{
        private string _CreateUserId;
     
        private string _Description;
        private DateTime? _EndTime;
        private int _QpaperId;
        private DateTime? _StartTime;

        private int _ApaperCount;

        private string _Subject;

        private DateTime _UpdateTime;

        /// <summary>
        /// 创建人/修改人
        ///  varchar(50)
        /// </summary>
        [MapField("create_user_id")]
        public string CreateUserId
        { get { return _CreateUserId; } set { _CreateUserId = value; OnPropertyChanged("create_user_id"); } }

       

        /// <summary>
        /// 说明
        ///  varchar(1000)
        /// </summary>
        [MapField("description"), Nullable]
        public string Description
        { get { return _Description; } set { _Description = value; OnPropertyChanged("description"); } }

        /// <summary>
        /// 调查截至时间
        ///  datetime
        /// </summary>
        [MapField("end_time"), Nullable]
        public DateTime? EndTime
        { get { return _EndTime; } set { _EndTime = value; OnPropertyChanged("end_time"); } }

        /// <summary>
        /// 问卷ID-自增
        ///  int(10)
        /// </summary>
        [MapField("qpaper_id"), Identity, PrimaryKey(1)] 
		public int QpaperId
		{ get{ return _QpaperId; } 	set{ _QpaperId = value ; } }


        [MapField("apaper_count")]
        public int ApaperCount
        {
            get { return _ApaperCount; }
            set { _ApaperCount = value; }
        }

        /// <summary>
        /// 调查开始时间
        ///  datetime
        /// </summary>
        [MapField("start_time"), Nullable]
        public DateTime? StartTime
        { get { return _StartTime; } set { _StartTime = value; OnPropertyChanged("start_time"); } }

        /// <summary>
        /// 主题
        ///  varchar(500)
        /// </summary>
        [MapField("subject")    ] 
		public string Subject
		{ get{ return _Subject; } 	set{ _Subject = value ;  OnPropertyChanged("subject"); } }
		/// <summary>
		/// 
		///  datetime
		/// </summary>
		[MapField("update_time")    ] 
		public DateTime UpdateTime
		{ get{ return _UpdateTime; } 	set{ _UpdateTime = value ;  OnPropertyChanged("update_time"); } }
	}

	[TableName("question")]
	public partial class Question : Survey.Service.InnerImpl.Domain.BaseEntity
	{
        private bool _ExtendInput;
        private string _Id;
        private string _ItemDetail;

        private int _PaperId;

        private sbyte _QuestionType;

        private int _Sequence;

        private string _Topic;

        /// <summary>
        /// 是否有自定义输入框
        ///  bit(1)
        /// </summary>
        [MapField("extend_input")]
        public bool ExtendInput
        { get { return _ExtendInput; } set { _ExtendInput = value; OnPropertyChanged("extend_input"); } }

        /// <summary>
        /// 问题表
        ///  varchar(50)
        /// </summary>
        [MapField("id"),   PrimaryKey(1)] 
		public string Id
		{ get{ return _Id; } 	set{ _Id = value ;  OnPropertyChanged("id"); } }
        /// <summary>
        /// 问题详细格式 格式同["选项1",“选项2”,"选项3"]
        ///  varchar(4000)
        /// </summary>
        [MapField("item_detail"), Nullable]
        public string ItemDetail
        { get { return _ItemDetail; } set { _ItemDetail = value; OnPropertyChanged("item_detail"); } }

        /// <summary>
        /// 
        ///  int(10)
        /// </summary>
        [MapField("paper_id")]
        public int PaperId
        { get { return _PaperId; } set { _PaperId = value; OnPropertyChanged("paper_id"); } }

        /// <summary>
        /// 0 单选题  1 多选题  2 问答题
        ///  tinyint(3)
        /// </summary>
        [MapField("question_type")]
        public sbyte QuestionType
        { get { return _QuestionType; } set { _QuestionType = value; OnPropertyChanged("question_type"); } }

        /// <summary>
        /// 排序号
        ///  int(10)
        /// </summary>
        [MapField("sequence")]
        public int Sequence
        { get { return _Sequence; } set { _Sequence = value; OnPropertyChanged("sequence"); } }

        /// <summary>
        /// 问题内容
        ///  varchar(500)
        /// </summary>
        [MapField("topic")    ] 
		public string Topic
		{ get{ return _Topic; } 	set{ _Topic = value ;  OnPropertyChanged("topic"); } }
	}

	[TableName("user_info")]
	public partial class UserInfo : Survey.Service.InnerImpl.Domain.BaseEntity
	{
        private string _Account;
        private DateTime _CreateTime;
        private string _FullName;
        private bool _IsAdmin;
        private string _Password;
        private DateTime _UpdateTime;
        private int _UserId;
        /// <summary>
        /// 用户登录账号
        ///  varchar(50)
        /// </summary>
        [MapField("account")]
        public string Account
        { get { return _Account; } set { _Account = value; OnPropertyChanged("account"); } }

        /// <summary>
        /// 创建时间
        ///  datetime
        /// </summary>
        [MapField("create_time")]
        public DateTime CreateTime
        { get { return _CreateTime; } set { _CreateTime = value; OnPropertyChanged("create_time"); } }

        /// <summary>
        /// 用户姓名
        ///  varchar(50)
        /// </summary>
        [MapField("full_name")]
        public string FullName
        { get { return _FullName; } set { _FullName = value; OnPropertyChanged("full_name"); } }

        /// <summary>
        /// 是否超级管理员
        ///  bit(1)
        /// </summary>
        [MapField("is_admin")]
        public bool IsAdmin
        { get { return _IsAdmin; } set { _IsAdmin = value; OnPropertyChanged("is_admin"); } }

        /// <summary>
        /// 登录密码
        ///  varchar(50)
        /// </summary>
        [MapField("password")]
        public string Password
        { get { return _Password; } set { _Password = value; OnPropertyChanged("password"); } }

        /// <summary>
        /// 最后更新时间
        ///  datetime
        /// </summary>
        [MapField("update_time")]
        public DateTime UpdateTime
        { get { return _UpdateTime; } set { _UpdateTime = value; OnPropertyChanged("update_time"); } }

        /// <summary>
        /// 用户ID 自增主键
        ///  int(10)
        /// </summary>
        [MapField("user_id"), Identity, PrimaryKey(1)] 
		public int UserId
		{ get{ return _UserId; } 	set{ _UserId = value ; } }
	}
}
