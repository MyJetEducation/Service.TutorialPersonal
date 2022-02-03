using System.Linq;
using Service.Core.Client.Education;
using Service.TutorialPersonal.Grpc.Models;

namespace Service.TutorialPersonal.Services
{
	public static class AnswerHelper
	{
		public static readonly EducationStructureTutorial Tutorial = EducationStructure.Tutorials[EducationTutorial.PersonalFinance];

		public const int MaxAnswerProgress = 100;
		public const int MinAnswerProgress = 0;

		public static int CheckAnswer(int progressPrc, PersonalTaskTestAnswerGrpcModel[] answers, int questionNumber, params int[] answerNumbers)
		{
			PersonalTaskTestAnswerGrpcModel answer = answers.FirstOrDefault(model => model.Number == questionNumber);

			return answer != null && answerNumbers.Intersect(answer.Value).Count() == answerNumbers.Length
				? progressPrc
				: MinAnswerProgress;
		}

		public static int CheckAnswer(int progressPrc, PersonalTaskTrueFalseAnswerGrpcModel[] answers, int questionNumber, bool answerValue)
		{
			PersonalTaskTrueFalseAnswerGrpcModel answer = answers.FirstOrDefault(model => model.Number == questionNumber);

			return answer != null && answer.Value == answerValue
				? progressPrc
				: MinAnswerProgress;
		}

		public static int GetSimpleProgress(bool taskPassed) => taskPassed
			? MaxAnswerProgress
			: MinAnswerProgress;
	}
}