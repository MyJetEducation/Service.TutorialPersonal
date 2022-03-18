using System.Runtime.Serialization;
using Service.Education;

namespace Service.TutorialPersonal.Grpc.Models.Task
{
	[DataContract]
	public class TaskTrueFalseAnswerGrpcModel : ITaskTrueFalseAnswer
	{
		[DataMember(Order = 1)]
		public int Number { get; set; }

		[DataMember(Order = 2)]
		public bool Value { get; set; }
	}
}