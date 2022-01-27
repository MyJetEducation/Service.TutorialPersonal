using System.Runtime.Serialization;
using Service.Core.Client.Constants;

namespace Service.TutorialPersonal.Grpc.Models.State
{
	[DataContract]
	public class TotalProgressStateGrpcModel
	{
		[DataMember(Order = 1)]
		public ProgressItemInfoGrpcModel Habit { get; set; }

		[DataMember(Order = 2)]
		public ProgressItemInfoGrpcModel Skill { get; set; }

		[DataMember(Order = 3)]
		public UserAchievement[] Achievements { get; set; }
	}
}