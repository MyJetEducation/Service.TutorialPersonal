using System.Runtime.Serialization;

namespace Service.TutorialPersonal.Grpc.Models.State
{
	[DataContract]
	public class TotalProgressStateGrpcModel
	{
		[DataMember(Order = 1)]
		public int HabitProgress { get; set; }

		[DataMember(Order = 2)]
		public string HabitName { get; set; }

		[DataMember(Order = 3)]
		public int SkillProgress { get; set; }

		[DataMember(Order = 4)]
		public string SkillName { get; set; }

		[DataMember(Order = 5)]
		public string[] Achievements { get; set; }
	}
}