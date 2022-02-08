using System.Linq;
using Service.Core.Client.Constants;
using Service.Core.Client.Education;
using Service.TutorialPersonal.Grpc.Models;

namespace Service.TutorialPersonal.Services
{
	public static class AnswerHelper
	{
		public static readonly EducationStructureTutorial Tutorial = EducationStructure.Tutorials[EducationTutorial.PersonalFinance];

		public static int CheckAnswer(int progressPrc, PersonalTaskTestAnswerGrpcModel[] answers, int questionNumber, params int[] answerNumbers)
		{
			PersonalTaskTestAnswerGrpcModel answer = answers.FirstOrDefault(model => model.Number == questionNumber);

			return CountProgress(answer != null && answerNumbers.Intersect(answer.Value).Count() == answerNumbers.Length, progressPrc);
		}

		public static int CheckAnswer(int progressPrc, PersonalTaskTrueFalseAnswerGrpcModel[] answers, int questionNumber, bool answerValue)
		{
			PersonalTaskTrueFalseAnswerGrpcModel answer = answers.FirstOrDefault(model => model.Number == questionNumber);

			return CountProgress(answer != null && answer.Value == answerValue, progressPrc);
		}

		public static int CountProgress(bool taskPassed, int progress = AnswerProgress.MaxAnswerProgress) => taskPassed ? progress : AnswerProgress.MinAnswerProgress;
	}
}