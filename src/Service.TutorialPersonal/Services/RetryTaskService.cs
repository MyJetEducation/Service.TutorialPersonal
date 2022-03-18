using System;
using System.Threading.Tasks;
using Service.Core.Client.Models;
using Service.Core.Client.Services;
using Service.EducationRetry.Grpc;
using Service.EducationRetry.Grpc.Models;
using Service.Grpc;
using Service.TutorialPersonal.Helper;

namespace Service.TutorialPersonal.Services
{
	public class RetryTaskService : IRetryTaskService
	{
		private readonly IGrpcServiceProxy<IEducationRetryService> _retryService;
		private readonly ISystemClock _systemClock;

		public RetryTaskService(IGrpcServiceProxy<IEducationRetryService> retryService, ISystemClock systemClock)
		{
			_retryService = retryService;
			_systemClock = systemClock;
		}

		public async ValueTask<bool> TaskInRetryStateAsync(Guid? userId, int unit, int task)
		{
			TaskRetryStateGrpcResponse response = await _retryService.Service.GetTaskRetryStateAsync(new GetTaskRetryStateGrpcRequest
			{
				UserId = userId,
				Tutorial = TutorialHelper.Tutorial,
				Unit = unit,
				Task = task
			});

			return response.InRetry;
		}

		public async ValueTask<DateTime?> GetRetryLastDateAsync(Guid? userId)
		{
			return (await _retryService.Service.GetRetryLastDateAsync(new GetRetryLastDateGrpcRequest
			{
				UserId = userId
			}))?.Date;
		}

		public bool CanRetryByTimeAsync(DateTime? progressDate, DateTime? lastRetryDate) => progressDate != null 
			&& OneDayGone(progressDate.Value) 
			&& (lastRetryDate == null || OneDayGone(lastRetryDate.Value));

		public async ValueTask<bool> HasRetryCountAsync(Guid? userId)
		{
			RetryCountGrpcResponse retryResponse = await _retryService.Service.GetRetryCountAsync(new GetRetryCountGrpcRequest
			{
				UserId = userId
			});

			return retryResponse?.Count > 0;
		}

		public async ValueTask<bool> ClearTaskRetryStateAsync(Guid? userId, int unit, int task)
		{
			CommonGrpcResponse decreased = await _retryService.TryCall(service => service.ClearTaskRetryStateAsync(new ClearTaskRetryStateGrpcRequest
			{
				UserId = userId,
				Tutorial = TutorialHelper.Tutorial,
				Unit = unit,
				Task = task
			}));

			return decreased.IsSuccess;
		}

		private bool OneDayGone(DateTime date) => _systemClock.Now.Subtract(date).TotalDays >= 1;
	}
}