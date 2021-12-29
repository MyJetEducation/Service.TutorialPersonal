using System;
using System.Runtime.Serialization;

namespace Service.TutorialPersonal.Grpc.Models
{
	[DataContract]
	public class PersonalSelectTaskUnitGrpcRequest
	{
		[DataMember(Order = 1)]
		public Guid? UserId { get; set; }
	}
}