using System.Runtime.Serialization;

namespace Service.TutorialPersonal.Grpc.Models.State
{
	[DataContract]
	public class TestScoreGrpcResponse
	{
		[DataMember(Order = 1)]
		public bool IsSuccess { get; set; }

		[DataMember(Order = 2)]
		public PersonalStateUnitGrpcModel Unit { get; set; }
	}
}