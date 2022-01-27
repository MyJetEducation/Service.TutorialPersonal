using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Service.Core.Client.Education;
using Service.TutorialPersonal.Grpc;
using Service.TutorialPersonal.Grpc.Models;
using Service.TutorialPersonal.Grpc.Models.State;
using Service.TutorialPersonal.Mappers;
using Service.UserProgress.Grpc;
using Service.UserProgress.Grpc.Models;
using Service.UserReward.Grpc;
using Service.UserReward.Grpc.Models;
using static Service.TutorialPersonal.Services.AnswerHelper;

namespace Service.TutorialPersonal.Services
{
	public class TutorialPersonalService : ITutorialPersonalService
	{
		private readonly ITaskProgressService _taskProgressService;
		private readonly IUserRewardService _userRewardService;
		private readonly IUserProgressService _userProgressService;

		public TutorialPersonalService(ITaskProgressService taskProgressService, IUserRewardService userRewardService, IUserProgressService userProgressService)
		{
			_taskProgressService = taskProgressService;
			_userRewardService = userRewardService;
			_userProgressService = userProgressService;
		}

		public async ValueTask<PersonalStateGrpcResponse> GetDashboardStateAsync(PersonalSelectTaskUnitGrpcRequest request)
		{
			var units = new List<PersonalStateUnitGrpcModel>();
			Guid? userId = request.UserId;

			foreach ((_, EducationStructureUnit unit) in Tutorial.Units)
			{
				PersonalStateUnitGrpcModel unitProgress = await _taskProgressService.GetUnitProgressAsync(userId, unit.Unit);
				if (unitProgress == null)
					break;

				units.Add(unitProgress);
			}

			UserAchievementsGrpcResponse achievements = await _userRewardService.GetUserAchievementsAsync(new GetUserAchievementsGrpcRequest {UserId = userId});

			return new PersonalStateGrpcResponse
			{
				Available = true,
				Units = units,
				TotalProgress = await GetTotalProgressStateGrpcModel(userId, achievements)
			};
		}

		public async ValueTask<FinishUnitGrpcResponse> GetFinishStateAsync(GetFinishStateGrpcRequest request)
		{
			Guid? userId = request.UserId;

			UserAchievementsGrpcResponse newAchievements = await _userRewardService.GetUserNewUnitAchievementsAsync(new GetUserAchievementsGrpcRequest {UserId = userId});
			PersonalStateUnitGrpcModel unitProgress = await _taskProgressService.GetUnitProgressAsync(userId, request.Unit);

			return new FinishUnitGrpcResponse
			{
				Unit = unitProgress,
				TotalProgress = await GetTotalProgressStateGrpcModel(userId, newAchievements)
			};
		}

		private async ValueTask<TotalProgressStateGrpcModel> GetTotalProgressStateGrpcModel(Guid? userId, UserAchievementsGrpcResponse achievementsGrpcResponse)
		{
			UnitedProgressGrpcResponse progress = await _userProgressService.GetUnitedProgressAsync(new GetProgressGrpcRequset
			{
				UserId = userId
			});

			return new TotalProgressStateGrpcModel
			{
				Habit = progress.Habit.ToGrpcModel(),
				Skill = progress.Skill.ToGrpcModel(),
				Achievements = achievementsGrpcResponse.Items
			};
		}

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