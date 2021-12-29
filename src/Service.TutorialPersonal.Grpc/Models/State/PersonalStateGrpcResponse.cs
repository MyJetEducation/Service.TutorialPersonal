using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Service.TutorialPersonal.Grpc.Models.State
{
	[DataContract]
	public class PersonalStateGrpcResponse
	{
		[DataMember(Order = 1)]
		public bool Available { get; set; }

		[DataMember(Order = 2)]
		public TimeSpan Duration { get; set; }

		[DataMember(Order = 3)]
		public IList<PersonalStateUnitGrpcModel> Units { get; set; }
	}
}