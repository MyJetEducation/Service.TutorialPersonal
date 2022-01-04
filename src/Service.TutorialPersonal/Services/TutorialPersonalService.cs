using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Service.Core.Domain.Extensions;
using Service.Core.Domain.Models.Education;
using Service.TutorialPersonal.Grpc;
using Service.TutorialPersonal.Grpc.Models;
using Service.TutorialPersonal.Grpc.Models.State;
using static Service.TutorialPersonal.Services.TutorialHelperService;

namespace Service.TutorialPersonal.Services
{
	public class TutorialPersonalService : ITutorialPersonalService
	{
		private static readonly EducationStructureTutorial Structure = EducationStructure.Tutorials[EducationTutorial.PersonalFinance];
		private static readonly EducationStructureUnit Unit1 = Structure.Units[1];

		private readonly ITutorialHelperService _tutorialHelperService;

		public TutorialPersonalService(ITutorialHelperService tutorialHelperService) => _tutorialHelperService = tutorialHelperService;

		public async ValueTask<PersonalStateGrpcResponse> GetDashboardStateAsync(PersonalSelectTaskUnitGrpcRequest request)
		{
			Guid? userId = request.UserId;
			var units = new List<PersonalStateUnitGrpcModel>();

			foreach ((_, EducationStructureUnit unit) in Structure.Units)
			{
				PersonalStateUnitGrpcModel unitProgress = await _tutorialHelperService.GetUnitProgressAsync(userId, unit);
				if (unitProgress == null)
					break;

				units.Add(unitProgress);
			}

			return new PersonalStateGrpcResponse
			{
				Available = true,
				Units = units,
				Duration = units.Sum(model => model.Duration),
				TotalProgress = new TotalProgressStateGrpcModel
				{
					HabitProgress = 20,
					HabitName = "The habit of forming savings",
					SkillProgress = 15,
					SkillName = "Skill SMART goals",
					Achievements = new[] {"Starter"}
				}
			};
		}

		public async ValueTask<TestScoreGrpcResponse> Unit1TextAsync(PersonalTaskTextGrpcRequest request)
		{
			return await _tutorialHelperService.SetTaskProgressAsync(request.UserId, Unit1, Unit1.Tasks[1], request.IsRetry, request.Duration);
		}

		public async ValueTask<TestScoreGrpcResponse> Unit1TestAsync(PersonalTaskTestGrpcRequest request)
		{
			PersonalTaskTestAnswerGrpcModel[] answers = request.Answers;

			float progress = CheckAnswer(20f, answers, 1, 1, 3)
				+ CheckAnswer(20f, answers, 2, 1, 3)
				+ CheckAnswer(20f, answers, 3, 3)
				+ CheckAnswer(20f, answers, 4, 1, 2, 3)
				+ CheckAnswer(20f, answers, 5, 1, 3);

			return await _tutorialHelperService.SetTaskProgressAsync(request.UserId, Unit1, Unit1.Tasks[2], request.IsRetry, request.Duration, progress);
		}

		public async ValueTask<TestScoreGrpcResponse> Unit1CaseAsync(PersonalTaskCaseGrpcRequest request)
		{
			float progress = request.Value == 1
				? MaxAnswerValuePrc
				: MinAnswerValuePrc;

			return await _tutorialHelperService.SetTaskProgressAsync(request.UserId, Unit1, Unit1.Tasks[3], request.IsRetry, request.Duration, progress);
		}

		public async ValueTask<TestScoreGrpcResponse> Unit1TrueFalseAsync(PersonalTaskTrueFalseGrpcRequest request)
		{
			PersonalTaskTrueFalseAnswerGrpcModel[] answers = request.Answers;

			float progress = CheckAnswer(20f, answers, 1, true)
				+ CheckAnswer(20f, answers, 2, true)
				+ CheckAnswer(20f, answers, 3, true)
				+ CheckAnswer(20f, answers, 4, false)
				+ CheckAnswer(20f, answers, 5, true);

			return await _tutorialHelperService.SetTaskProgressAsync(request.UserId, Unit1, Unit1.Tasks[4], request.IsRetry, request.Duration, progress);
		}

		public async ValueTask<TestScoreGrpcResponse> Unit1GameAsync(PersonalTaskGameGrpcRequest request)
		{
			return await _tutorialHelperService.SetTaskProgressAsync(request.UserId, Unit1, Unit1.Tasks[5], request.IsRetry, request.Duration);
		}

		public async ValueTask<FinishUnitGrpcResponse> GetFinishStateAsync(GetFinishStateGrpcRequest request) => new FinishUnitGrpcResponse
		{
			Unit = await _tutorialHelperService.GetUnitProgressAsync(request.UserId, Structure.Units[request.Unit]),
			TotalProgress = new TotalProgressStateGrpcModel
			{
				HabitProgress = 20,
				HabitName = "The habit of forming savings",
				SkillProgress = 15,
				SkillName = "Skill SMART goals",
				Achievements = new[] {"Starter", "Viola", "Ignition"}
			}
		};
	}
}