﻿using System.Linq;
using Service.Core.Domain.Models.Education;
using Service.TutorialPersonal.Grpc.Models;

namespace Service.TutorialPersonal.Services
{
	public static class AnswerHelper
	{
		public static readonly EducationStructureTutorial Tutorial = EducationStructure.Tutorials[EducationTutorial.PersonalFinance];
		public static readonly EducationStructureUnit Unit1 = Tutorial.Units[1];

		public const int MaxAnswerProgress = 100;
		public const int MinAnswerProgress = 0;

		public static float CheckAnswer(float progress, PersonalTaskTestAnswerGrpcModel[] answers, int questionNumber, params int[] answerNumbers)
		{
			PersonalTaskTestAnswerGrpcModel answer = answers.FirstOrDefault(model => model.Number == questionNumber);

			return answer != null && answerNumbers.Intersect(answer.Value).Count() == answerNumbers.Length
				? progress
				: MinAnswerProgress;
		}

		public static float CheckAnswer(float progress, PersonalTaskTrueFalseAnswerGrpcModel[] answers, int questionNumber, bool answerValue)
		{
			PersonalTaskTrueFalseAnswerGrpcModel answer = answers.FirstOrDefault(model => model.Number == questionNumber);

			return answer != null && answer.Value == answerValue
				? progress
				: MinAnswerProgress;
		}
	}
}