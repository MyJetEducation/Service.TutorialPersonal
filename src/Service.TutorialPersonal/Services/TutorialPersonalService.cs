using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Service.Core.Client.Education;
using Service.TutorialPersonal.Grpc;
using Service.TutorialPersonal.Grpc.Models;
using Service.TutorialPersonal.Grpc.Models.State;
using Service.TutorialPersonal.Mappers;
using Service.TutorialPersonal.Models;
using Service.UserProgress.Grpc;
using Service.UserProgress.Grpc.Models;
using Service.UserReward.Grpc;
using Service.UserReward.Grpc.Models;
using static Service.TutorialPersonal.Services.AnswerHelper;

namespace Service.TutorialPersonal.Services
{
	public partial class TutorialPersonalService : ITutorialPersonalService
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
				UnitInfoModel unitInfo = await _taskProgressService.GetUnitProgressAsync(userId, unit.Unit);
				if (unitInfo == null)
					break;

				units.Add(unitInfo.PersonalStateUnit);
			}

			UserAchievementsGrpcResponse achievements = await _userRewardService.GetUserAchievementsAsync(new GetUserAchievementsGrpcRequest {UserId = userId});
			UnitedProgressGrpcResponse progress = await _userProgressService.GetUnitedProgressAsync(new GetProgressGrpcRequset {UserId = userId});

			return new PersonalStateGrpcResponse
			{
				Available = true,
				Units = units,
				TotalProgress = new TotalProgressStateGrpcModel
				{
					Habit = progress.Habit.ToGrpcModel(),
					Skill = progress.Skill.ToGrpcModel(),
					Achievements = achievements.Items
				}
			};
		}

		public async ValueTask<FinishUnitGrpcResponse> GetFinishStateAsync(GetFinishStateGrpcRequest request)
		{
			Guid? userId = request.UserId;
			var result = new FinishUnitGrpcResponse();

			UserAchievementsGrpcResponse newAchievements = await _userRewardService.GetUserNewUnitAchievementsAsync(new GetUserAchievementsGrpcRequest {UserId = userId});
			if (newAchievements != null)
				result.NewAchievements = newAchievements.Items;

			UnitInfoModel unitInfo = await _taskProgressService.GetUnitProgressAsync(userId, request.Unit);
			if (unitInfo == null)
				return result;

			result.Unit = unitInfo.PersonalStateUnit;
			result.TrueFalseProgress = unitInfo.TrueFalseProgress;
			result.CaseProgress = unitInfo.CaseProgress;

			return result;
		}
	}
}