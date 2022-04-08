using System;
using System.Runtime.Serialization;

namespace Service.TutorialPersonal.Grpc.Models.Task
{
	[DataContract]
	public class TaskTrueFalseGrpcRequest
	{
		[DataMember(Order = 1)]
		public string UserId { get; set; }

		[DataMember(Order = 2)]
		public TaskTrueFalseAnswerGrpcModel[] Answers { get; set; }

		[DataMember(Order = 3)]
		public bool IsRetry { get; set; }

		[DataMember(Order = 4)]
		public TimeSpan Duration { get; set; }
	}
}