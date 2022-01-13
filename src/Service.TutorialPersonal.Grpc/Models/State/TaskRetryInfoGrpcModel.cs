using System.Runtime.Serialization;

namespace Service.TutorialPersonal.Grpc.Models.State
{
	[DataContract]
	public class TaskRetryInfoGrpcModel
	{
		[DataMember(Order = 1)]
		public bool InRetry { get; set; }

		[DataMember(Order = 2)]
		public bool CanRetryByTime { get; set; }

		[DataMember(Order = 3)]
		public bool CanRetryByCount { get; set; }
	}
}