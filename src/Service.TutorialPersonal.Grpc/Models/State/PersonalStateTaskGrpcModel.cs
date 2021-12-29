using System;
using System.Runtime.Serialization;

namespace Service.TutorialPersonal.Grpc.Models.State
{
	[DataContract]
	public class PersonalStateTaskGrpcModel
	{
		[DataMember(Order = 1)]
		public int TaskId { get; set; }

		[DataMember(Order = 2)]
		public TimeSpan Duration { get; set; }

		[DataMember(Order = 3)]
		public int TestScore { get; set; }

		[DataMember(Order = 4)]
		public bool CanRetry { get; set; }
	}
}