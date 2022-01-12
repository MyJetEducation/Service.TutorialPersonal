using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Service.Core.Domain.Models;
using Service.Core.Domain.Models.Education;
using Service.Core.Grpc.Models;
using Service.EducationProgress.Grpc;
using Service.EducationProgress.Grpc.Models;
using Service.EducationRetry.Grpc;
using Service.EducationRetry.Grpc.Models;
using Service.TutorialPersonal.Grpc.Models.State;

namespace Service.TutorialPersonal.Services
{
	public class TutorialHelperService : ITutorialHelperService
	{
		private readonly IEducationProgressService _progressService;
		private readonly IEducationRetryService _retryService;
		private readonly ISystemClock _systemClock;
		private readonly ILogger<TutorialHelperService> _logger;

		public TutorialHelperService(IEducationProgressService progressService, IEducationRetryService retryService, ISystemClock systemClock, ILogger<TutorialHelperService> logger)
		{
			_progressService = progressService;
			_retryService = retryService;
			_systemClock = systemClock;
			_logger = logger;
		}

		public async ValueTask<TestScoreGrpcResponse> SetTaskProgressAsync(Guid? userId, EducationStructureUnit unit, EducationStructureTask task, bool isRetry, TimeSpan duration, int? progress = null)
		{
			if (userId == null
				|| !await ValidatePostition(userId, unit, task)
				|| !await ValidateProgress(userId, unit, isRetry, task.Task))
				return new TestScoreGrpcResponse {IsSuccess = false};

			_logger.LogDebug("Try to set progress for user {userId}...", userId);

			CommonGrpcResponse response = await _progressService.SetProgressAsync(new SetEducationProgressGrpcRequest
			{
				UserId = userId,
				Tutorial = EducationTutorial.PersonalFinance,
				Unit = unit.Unit,
				Task = task.Task,
				Value = progress ?? AnswerHelper.MaxAnswerProgress,
				Duration = duration
			});

			_logger.LogDebug("Result: {response}...", response.IsSuccess);

			return new TestScoreGrpcResponse
			{
				IsSuccess = response.IsSuccess,
				Unit = await GetUnitProgressAsync(userId, unit.Unit)
			};
		}

		private async ValueTask<bool> ValidateProgress(Guid? userId, EducationStructureUnit unit, bool isRetry, int task)
		{
			TaskEducationProgressGrpcModel taskProgress = await GetTaskProgressAsync(userId, unit.Unit, task);

			//retry without normal answered task
			if (isRetry && taskProgress is { HasProgress: false })
				return false;

			//retry 100% score task
			if (isRetry && taskProgress is { HasProgress: true, Value: AnswerHelper.MaxAnswerProgress })
				return false;

			//can't retry (by date or has no retry-count)
			if (isRetry && !await GetRetryResultAsync(taskProgress, userId, unit))
				return false;

			//answer already answered task
			if (!isRetry && taskProgress is { HasProgress: true })
				return false;

			return true;
		}

		private async ValueTask<bool> ValidatePostition(Guid? userId, EducationStructureUnit unit, EducationStructureTask task)
		{
			if (unit.Unit == 1 && task.Task == 1)
				return true;

			EducationStructureUnit prevUnit = AnswerHelper.Tutorial.Units[unit.Unit];
			EducationStructureTask prevTask;

			if (unit.Unit > 1 && task.Task == 1)
			{
				prevUnit = AnswerHelper.Tutorial.Units[unit.Unit - 1];
				prevTask = unit.Tasks[unit.Tasks.Values.Count - 1];
			}
			else
				prevTask = unit.Tasks[task.Task - 1];

			TaskEducationProgressGrpcModel progress = await GetTaskProgressAsync(userId, prevUnit.Unit, prevTask.Task);

			bool progressHasProgress = progress?.HasProgress == true;
			if (!progressHasProgress)
				_logger.LogError("Invalid position while set task progress for user {userId}", userId);

			return progressHasProgress;
		}

		public async ValueTask<PersonalStateUnitGrpcModel> GetUnitProgressAsync(Guid? userId, int unit)
		{
			EducationProgressGrpcResponse unitProgressResponse = await _progressService.GetProgressAsync(new GetEducationProgressGrpcRequest
			{
				Tutorial = EducationTutorial.PersonalFinance,
				Unit = unit,
				UserId = userId
			});

			int unitProgress = (unitProgressResponse?.Value).GetValueOrDefault();
			if (unitProgress == 0)
				return null;

			var tasks = new List<PersonalStateTaskGrpcModel>();
			EducationStructureUnit structureUnit = EducationHelper.GetUnit(EducationTutorial.PersonalFinance, unit);

			foreach ((_, EducationStructureTask structureTask) in structureUnit.Tasks)
			{
				int task = structureTask.Task;

				TaskEducationProgressGrpcModel taskProgress = await GetTaskProgressAsync(userId, unit, task);
				if (!(taskProgress is { HasProgress: true }))
					break;

				tasks.Add(new PersonalStateTaskGrpcModel
				{
					Task = task,
					TestScore = taskProgress.Value,
					CanRetry = CanRetryByTime(taskProgress) || await HasRetryCountAsync(userId)
				});
			}

			return new PersonalStateUnitGrpcModel
			{
				Unit = unit,
				TestScore = unitProgress,
				Tasks = tasks
			};
		}

		public async ValueTask<TaskEducationProgressGrpcModel> GetTaskProgressAsync(Guid? userId, int unit, int task)
		{
			TaskEducationProgressGrpcResponse taskProgressResponse = await _progressService.GetTaskProgressAsync(new GetTaskEducationProgressGrpcRequest
			{
				Tutorial = EducationTutorial.PersonalFinance,
				Unit = unit,
				Task = task,
				UserId = userId
			});

			return taskProgressResponse?.Progress;
		}

		public async ValueTask<bool> HasRetryCountAsync(Guid? userId)
		{
			RetryCountGrpcResponse retryResponse = await _retryService.GetRetryCountAsync(new GetRetryCountGrpcRequest
			{
				UserId = userId
			});

			return retryResponse?.Count > 0;
		}

		public async ValueTask<bool> GetRetryResultAsync(TaskEducationProgressGrpcModel taskProgress, Guid? userId, EducationStructureUnit unit)
		{
			if (CanRetryByTime(taskProgress))
				return true;

			if (!await HasRetryCountAsync(userId))
				return false;

			CommonGrpcResponse retryDecreaseResponse = await _retryService.DecreaseRetryCountAsync(new DecreaseRetryCountGrpcRequest
			{
				UserId = userId,
				Tutorial = EducationTutorial.PersonalFinance,
				Unit = unit.Unit,
				Task = unit.Tasks[1].Task,
				Value = 1
			});

			return !retryDecreaseResponse.IsSuccess;
		}

		private bool CanRetryByTime(TaskEducationProgressGrpcModel progressGrpcModel)
		{
			DateTime? whenFinished = progressGrpcModel.Date;

			return whenFinished != null && _systemClock.Now.Subtract(whenFinished.Value).TotalDays >= 1;
		}
	}
}