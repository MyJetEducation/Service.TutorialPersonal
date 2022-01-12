using System.Runtime.Serialization;

namespace Service.TutorialPersonal.Grpc.Models.State
{
	[DataContract]
	public class TotalProgressStateGrpcModel
	{
		[DataMember(Order = 1)]
		public int HabitProgress { get; set; }

		[DataMember(Order = 2)]
		public int HabitValue { get; set; }

		[DataMember(Order = 3)]
		public int SkillProgress { get; set; }

		[DataMember(Order = 4)]
		public int SkillValue { get; set; }

		[DataMember(Order = 5)]
		public string[] Achievements { get; set; }
	}
}