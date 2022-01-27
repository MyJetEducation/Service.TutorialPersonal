using System.Runtime.Serialization;

namespace Service.TutorialPersonal.Grpc.Models.State
{
	[DataContract]
	public class ProgressItemInfoGrpcModel
	{
		[DataMember(Order = 1)]
		public int Index { get; set; }

		[DataMember(Order = 2)]
		public int Count { get; set; }

		[DataMember(Order = 3)]
		public int Progress { get; set; }
	}
}