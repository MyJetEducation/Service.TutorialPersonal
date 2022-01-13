﻿using System.Runtime.Serialization;

namespace Service.TutorialPersonal.Grpc.Models.State
{
	[DataContract]
	public class PersonalStateUnitGrpcModel
	{
		[DataMember(Order = 1)]
		public int Unit { get; set; }

		[DataMember(Order = 2)]
		public int TestScore { get; set; }

		[DataMember(Order = 3)]
		public PersonalStateTaskGrpcModel[] Tasks { get; set; }
	}
}