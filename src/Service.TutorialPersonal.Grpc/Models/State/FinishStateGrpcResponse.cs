using System.Runtime.Serialization;
using Service.Core.Client.Constants;

namespace Service.TutorialPersonal.Grpc.Models.State
{
	[DataContract]
	public class FinishStateGrpcResponse
	{
		[DataMember(Order = 1)]
		public int Test { get; set; }

		[DataMember(Order = 2)]
		public int TrueFalse { get; set; }

		[DataMember(Order = 3)]
		public int Case { get; set; }

		[DataMember(Order = 4)]
		public UserAchievement[] Achievements { get; set; }
	}
}