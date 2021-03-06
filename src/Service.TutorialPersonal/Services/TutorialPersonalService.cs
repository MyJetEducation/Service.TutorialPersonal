using System;
using System.Threading.Tasks;
using Service.Education.Contracts.State;
using Service.TutorialPersonal.Grpc;
using Service.TutorialPersonal.Mappers;
using Service.TutorialPersonal.Models;
using Service.UserReward.Grpc;
using Service.UserReward.Grpc.Models;

namespace Service.TutorialPersonal.Services
{
	public partial class TutorialPersonalService : ITutorialPersonalService
	{
		private readonly ITaskProgressService _taskProgressService;
		private readonly IUserRewardService _userRewardService;

		public TutorialPersonalService(ITaskProgressService taskProgressService, IUserRewardService userRewardService)
		{
			_taskProgressService = taskProgressService;
			_userRewardService = userRewardService;
		}

		public async ValueTask<FinishStateGrpcResponse> GetFinishStateAsync(GetFinishStateGrpcRequest request)
		{
			string userId = request.UserId;
			int? unit = request.Unit;

			TaskTypeProgressInfo typeProgressInfo = await _taskProgressService.GetTotalProgressAsync(userId, unit);

			UserAchievementsGrpcResponse achievements = await _userRewardService.GetUserNewAchievementsAsync(new GetUserAchievementsGrpcRequest
			{
				UserId = userId,
				Unit = unit
			});

			return typeProgressInfo.ToGrpcModel(achievements?.Items);
		}
	}
}