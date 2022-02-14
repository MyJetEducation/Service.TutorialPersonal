using System.Runtime.Serialization;
using Service.Core.Client.Constants;

namespace Service.TutorialPersonal.Grpc.Models.State
{
	[DataContract]
	public class DashboardStateGrpcResponse
	{
		[DataMember(Order = 1)]
		public int Test { get; set; }

		[DataMember(Order = 2)]
		public int TrueFalse { get; set; }

		[DataMember(Order = 3)]
		public int Case { get; set; }

		[DataMember(Order = 4)]
		public int Game { get; set; }

		[DataMember(Order = 5)]
		public int Video { get; set; }

		[DataMember(Order = 6)]
		public int Text { get; set; }

		[DataMember(Order = 7)]
		public UserAchievement[] Achievements { get; set; }
	}
}