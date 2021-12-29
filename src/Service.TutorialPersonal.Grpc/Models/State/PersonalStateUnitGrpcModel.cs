using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Service.TutorialPersonal.Grpc.Models.State
{
	[DataContract]
	public class PersonalStateUnitGrpcModel
	{
		[DataMember(Order = 1)]
		public int Index { get; set; }

		[DataMember(Order = 2)]
		public TimeSpan Duration { get; set; }

		[DataMember(Order = 3)]
		public int TestScore { get; set; }

		[DataMember(Order = 4)]
		public IList<PersonalStateTaskGrpcModel> Tasks { get; set; }

		[DataMember(Order = 5)]
		public int HabitCount { get; set; }

		[DataMember(Order = 6)]
		public int SkillCount { get; set; }
	}
}