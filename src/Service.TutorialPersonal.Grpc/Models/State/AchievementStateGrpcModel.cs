using System.Runtime.Serialization;

namespace Service.TutorialPersonal.Grpc.Models.State
{
	[DataContract]
	public class AchievementStateGrpcModel
	{
		[DataMember(Order = 1)]
		public string[] Achievements { get; set; }
	}
}