using System.Runtime.Serialization;

namespace Service.TutorialPersonal.Grpc.Models
{
	[DataContract]
	public class PersonalTaskTrueFalseAnswerGrpcModel
	{
		[DataMember(Order = 1)]
		public int Number { get; set; }

		[DataMember(Order = 2)]
		public bool Value { get; set; }
	}
}