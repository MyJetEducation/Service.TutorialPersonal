using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Service.Core.Client.Education;
using Service.Core.Client.Models;
using Service.EducationProgress.Grpc;
using Service.EducationProgress.Grpc.Models;
using Service.TutorialPersonal.Grpc.Models.State;

namespace Service.TutorialPersonal.Services
{
	public class TaskProgressService : ITaskProgressService
	{
		private readonly IEducationProgressService _progressService;
		private readonly ILogger<TaskProgressService> _logger;
		private readonly IRetryTaskService _retryTaskService;

		public TaskProgressService(IEducationProgressService progressService, ILogger<TaskProgressService> logger, IRetryTaskService retryTaskService)
		{
			_progressService = progressService;
			_logger = logger;
			_retryTaskService = retryTaskService;
		}

		public async ValueTask<TestScoreGrpcResponse> SetTaskProgressAsync(Guid? userId, EducationStructureUnit unit, EducationStructureTask task, bool isRetry, TimeSpan duration, int? progress = null)
		{
			int taskId = task.Task;
			int unitId = unit.Unit;

			if (userId == null
				|| !await ValidatePostition(userId, unit, taskId)
				|| !await ValidateProgress(userId, unitId, taskId, isRetry))
				return new TestScoreGrpcResponse {IsSuccess = false};

			_logger.LogDebug("Try to set progress for user {userId}...", userId);

			CommonGrpcResponse response = await _progressService.SetProgressAsync(new SetEducationProgressGrpcRequest
			{
				UserId = userId,
				Tutorial = EducationTutorial.PersonalFinance,
				Unit = unitId,
				Task = taskId,
				Value = progress ?? AnswerHelper.MaxAnswerProgress,
				Duration = duration,
				IsRetry = isRetry
			});

			_logger.LogDebug("Result: {response}...", response.IsSuccess);

			if (isRetry)
			{
				bool cleared = await _retryTaskService.ClearTaskRetryStateAsync(userId, unitId, taskId);
				if (!cleared)
					_logger.LogError("Error while clearing retry state for user {user}, unit: {unit}, task: {task}.", userId, unitId, taskId);
			}

			return new TestScoreGrpcResponse
			{
				IsSuccess = response.IsSuccess,
				Unit = await GetUnitProgressAsync(userId, unitId)
			};
		}

		private async ValueTask<bool> ValidateProgress(Guid? userId, int unit, int task, bool isRetry)
		{
			if (Program.ReloadedSettings(model => model.TestMode).Invoke())
				return true;

			TaskEducationProgressGrpcModel taskProgress = await GetTaskProgressAsync(userId, unit, task);

			//retry without normal answered task
			if (isRetry && taskProgress is { HasProgress: false })
				return false;

			//retry 100% score task
			if (isRetry && taskProgress is { HasProgress: true, Value: AnswerHelper.MaxAnswerProgress })
				return false;

			//can't retry (by date or has no retry-count)
			if (isRetry && !await _retryTaskService.TaskInRetryStateAsync(userId, unit, task))
				return false;

			//answer already answered task
			if (!isRetry && taskProgress is { HasProgress: true })
				return false;

			return true;
		}

		private async ValueTask<bool> ValidatePostition(Guid? userId, EducationStructureUnit unit, int task)
		{
			if (Program.ReloadedSettings(model => model.TestMode).Invoke())
				return true;

			if (unit.Unit == 1 && task == 1)
				return true;

			EducationStructureUnit prevUnit = AnswerHelper.Tutorial.Units[unit.Unit];
			EducationStructureTask prevTask;

			if (unit.Unit > 1 && task == 1)
			{
				prevUnit = AnswerHelper.Tutorial.Units[unit.Unit - 1];
				prevTask = unit.Tasks[unit.Tasks.Values.Count - 1];
			}
			else
				prevTask = unit.Tasks[task - 1];

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
				int taskId = structureTask.Task;

				TaskEducationProgressGrpcModel taskProgress = await GetTaskProgressAsync(userId, unit, taskId);
				if (!(taskProgress is { HasProgress: true }))
					break;

				int progressValue = taskProgress.Value;
				bool lowProgress = progressValue < 100;
				bool inRetryState = await _retryTaskService.TaskInRetryStateAsync(userId, unit, taskId);
				bool canRetryTask = !inRetryState && lowProgress;

				tasks.Add(new PersonalStateTaskGrpcModel
				{
					Task = taskId,
					TestScore = progressValue,
					RetryInfo = new TaskRetryInfoGrpcModel
					{
						InRetry = inRetryState,
						CanRetryByCount = canRetryTask && await _retryTaskService.HasRetryCountAsync(userId),
						CanRetryByTime = canRetryTask && await _retryTaskService.CanRetryByTimeAsync(userId, taskProgress)
					}
				});
			}

			return new PersonalStateUnitGrpcModel
			{
				Unit = unit,
				TestScore = unitProgress,
				Tasks = tasks.ToArray()
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
	}
}