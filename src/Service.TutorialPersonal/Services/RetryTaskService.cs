using System;
using System.Threading.Tasks;
using Service.Core.Client.Models;
using Service.Core.Client.Services;
using Service.Education.Structure;
using Service.EducationRetry.Grpc;
using Service.EducationRetry.Grpc.Models;

namespace Service.TutorialPersonal.Services
{
	public class RetryTaskService : IRetryTaskService
	{
		private readonly IEducationRetryService _retryService;
		private readonly ISystemClock _systemClock;

		public RetryTaskService(IEducationRetryService retryService, ISystemClock systemClock)
		{
			_retryService = retryService;
			_systemClock = systemClock;
		}

		public async ValueTask<bool> TaskInRetryStateAsync(Guid? userId, int unit, int task)
		{
			TaskRetryStateGrpcResponse response = await _retryService.GetTaskRetryStateAsync(new GetTaskRetryStateGrpcRequest
			{
				UserId = userId,
				Tutorial = EducationTutorial.PersonalFinance,
				Unit = unit,
				Task = task
			});

			return response.InRetry;
		}

		public async ValueTask<bool> CanRetryByTimeAsync(Guid? userId, DateTime? progressDate)
		{
			if (progressDate == null || !OneDayGone(progressDate.Value))
				return false;

			RetryLastDateGrpcResponse response = await _retryService.GetRetryLastDateAsync(new GetRetryLastDateGrpcRequest
			{
				UserId = userId
			});

			DateTime? date = response?.Date;

			return date == null || OneDayGone(date.Value);
		}

		public async ValueTask<bool> HasRetryCountAsync(Guid? userId)
		{
			RetryCountGrpcResponse retryResponse = await _retryService.GetRetryCountAsync(new GetRetryCountGrpcRequest
			{
				UserId = userId
			});

			return retryResponse?.Count > 0;
		}

		public async ValueTask<bool> ClearTaskRetryStateAsync(Guid? userId, int unit, int task)
		{
			CommonGrpcResponse decreased = await _retryService.ClearTaskRetryStateAsync(new ClearTaskRetryStateGrpcRequest
			{
				UserId = userId,
				Tutorial = EducationTutorial.PersonalFinance,
				Unit = unit,
				Task = task
			});

			return decreased.IsSuccess;
		}

		private bool OneDayGone(DateTime date) => _systemClock.Now.Subtract(date).Days >= 1;
	}
}