using System.Runtime.Serialization;

namespace Service.TutorialPersonal.Grpc.Models.State
{
	[DataContract]
	public class FinishUnitGrpcResponse
	{
		[DataMember(Order = 1)]
		public PersonalStateUnitGrpcModel Unit { get; set; }

		[DataMember(Order = 2)]
		public AchievementStateGrpcModel Achievements { get; set; }

		[DataMember(Order = 3)]
		public TotalProgressStateGrpcModel TotalProgress { get; set; }
	}
}