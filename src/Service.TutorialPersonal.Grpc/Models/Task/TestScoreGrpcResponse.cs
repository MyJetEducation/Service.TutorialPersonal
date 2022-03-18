using System.Runtime.Serialization;
using Service.TutorialPersonal.Grpc.Models.State;

namespace Service.TutorialPersonal.Grpc.Models.Task
{
	[DataContract]
	public class TestScoreGrpcResponse
	{
		[DataMember(Order = 1)]
		public bool IsSuccess { get; set; }

		[DataMember(Order = 2)]
		public UnitStateGrpcModel Unit { get; set; }
	}
}