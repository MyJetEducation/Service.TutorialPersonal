using System.Collections.Generic;
using System.Threading.Tasks;
using Service.Core.Domain.Models.Education;
using Service.TutorialPersonal.Grpc;
using Service.TutorialPersonal.Grpc.Models;
using Service.TutorialPersonal.Grpc.Models.State;
using static Service.TutorialPersonal.Services.AnswerHelper;

namespace Service.TutorialPersonal.Services
{
	public class TutorialPersonalService : ITutorialPersonalService
	{
		private readonly ITaskProgressService _taskProgressService;

		public TutorialPersonalService(ITaskProgressService taskProgressService) => _taskProgressService = taskProgressService;

		public async ValueTask<PersonalStateGrpcResponse> GetDashboardStateAsync(PersonalSelectTaskUnitGrpcRequest request)
		{
			var units = new List<PersonalStateUnitGrpcModel>();

			foreach ((_, EducationStructureUnit unit) in Tutorial.Units)
			{
				PersonalStateUnitGrpcModel unitProgress = await _taskProgressService.GetUnitProgressAsync(request.UserId, unit.Unit);
				if (unitProgress == null)
					break;

				units.Add(unitProgress);
			}

			return new PersonalStateGrpcResponse
			{
				Available = true,
				Units = units,
				TotalProgress = new TotalProgressStateGrpcModel
				{
					HabitValue = 1,
					HabitProgress = 20,
					SkillValue = 1,
					SkillProgress = 15,
					Achievements = new[] {"Starter"}
				}
			};
		}

		public async ValueTask<FinishUnitGrpcResponse> GetFinishStateAsync(GetFinishStateGrpcRequest request) => new FinishUnitGrpcResponse
		{
			Unit = await _taskProgressService.GetUnitProgressAsync(request.UserId, request.Unit),
			TotalProgress = new TotalProgressStateGrpcModel
			{
				HabitValue = 1,
				HabitProgress = 20,
				SkillValue = 1,
				SkillProgress = 15,
				Achievements = new[] {"Starter", "Viola", "Ignition"}
			}
		};

		#region Unit1 tasks

		public async ValueTask<TestScoreGrpcResponse> Unit1TextAsync(PersonalTaskTextGrpcRequest request)
		{
			return await _taskProgressService.SetTaskProgressAsync(request.UserId, Unit1, Unit1.Tasks[1], request.IsRetry, request.Duration);
		}

		public async ValueTask<TestScoreGrpcResponse> Unit1TestAsync(PersonalTaskTestGrpcRequest request)
		{
			PersonalTaskTestAnswerGrpcModel[] answers = request.Answers;

			int progress = CheckAnswer(20, answers, 1, 1, 3)
				+ CheckAnswer(20, answers, 2, 1, 3)
				+ CheckAnswer(20, answers, 3, 3)
				+ CheckAnswer(20, answers, 4, 1, 2, 3)
				+ CheckAnswer(20, answers, 5, 1, 3);

			return await _taskProgressService.SetTaskProgressAsync(request.UserId, Unit1, Unit1.Tasks[2], request.IsRetry, request.Duration, progress);
		}

		public async ValueTask<TestScoreGrpcResponse> Unit1CaseAsync(PersonalTaskCaseGrpcRequest request)
		{
			int progress = request.Value == 1
				? MaxAnswerProgress
				: MinAnswerProgress;

			return await _taskProgressService.SetTaskProgressAsync(request.UserId, Unit1, Unit1.Tasks[3], request.IsRetry, request.Duration, progress);
		}

		public async ValueTask<TestScoreGrpcResponse> Unit1TrueFalseAsync(PersonalTaskTrueFalseGrpcRequest request)
		{
			PersonalTaskTrueFalseAnswerGrpcModel[] answers = request.Answers;

			int progress = CheckAnswer(20, answers, 1, true)
				+ CheckAnswer(20, answers, 2, true)
				+ CheckAnswer(20, answers, 3, true)
				+ CheckAnswer(20, answers, 4, false)
				+ CheckAnswer(20, answers, 5, true);

			return await _taskProgressService.SetTaskProgressAsync(request.UserId, Unit1, Unit1.Tasks[4], request.IsRetry, request.Duration, progress);
		}

		public async ValueTask<TestScoreGrpcResponse> Unit1GameAsync(PersonalTaskGameGrpcRequest request)
		{
			return await _taskProgressService.SetTaskProgressAsync(request.UserId, Unit1, Unit1.Tasks[5], request.IsRetry, request.Duration);
		}

		#endregion
	}
}