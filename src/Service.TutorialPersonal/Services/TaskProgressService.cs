using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Service.Core.Client.Models;
using Service.Education.Constants;
using Service.Education.Extensions;
using Service.Education.Helpers;
using Service.Education.Structure;
using Service.EducationProgress.Grpc;
using Service.EducationProgress.Grpc.Models;
using Service.TutorialPersonal.Grpc.Models.State;
using Service.TutorialPersonal.Helper;
using Service.TutorialPersonal.Models;

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
				|| !await ValidateProgress(userId, unitId, task, isRetry))
				return new TestScoreGrpcResponse { IsSuccess = false };

			_logger.LogDebug("Try to set progress for user {userId}...", userId);

			CommonGrpcResponse response = await _progressService.SetProgressAsync(new SetEducationProgressGrpcRequest
			{
				UserId = userId,
				Tutorial = TutorialHelper.Tutorial,
				Unit = unitId,
				Task = taskId,
				Value = progress ?? Progress.MaxProgress,
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

		private async ValueTask<bool> ValidateProgress(Guid? userId, int unit, EducationStructureTask task, bool isRetry)
		{
			TaskEducationProgressGrpcModel taskProgress = await GetTaskProgressAsync(userId, unit, task.Task);
			bool notGame = task.TaskType != EducationTaskType.Game;

			//retry without normal answered task
			if (isRetry && taskProgress is { HasProgress: false })
				return false;

			//retry 100% score task (exclude game)
			if (isRetry && notGame && taskProgress is { HasProgress: true, Value: Progress.MaxProgress })
				return false;

			//can't retry (by date or has no retry-count)
			if (isRetry && !await _retryTaskService.TaskInRetryStateAsync(userId, unit, task.Task))
				return false;

			//answer already answered task
			if (!isRetry && taskProgress is { HasProgress: true })
				return false;

			return true;
		}

		private async ValueTask<bool> ValidatePostition(Guid? userId, EducationStructureUnit unit, int taskIndex)
		{
			int unitIndex = unit.Unit;

			//If start unit
			if (taskIndex == 1)
			{
				//First - accept
				if (unitIndex == 1)
					return true;

				//Else - if prev unit all tasks score >=80
				int prevUnitIndex = unitIndex - 1;
				UnitStateGrpcModel prevUnitProgress = await GetUnitProgressAsync(userId, prevUnitIndex);
				if (prevUnitProgress == null)
				{
					_logger.LogError("Can't get progress of previous unit ({unit}) for user {userId}", prevUnitIndex, userId);
					return false;
				}

				bool prevUnitIsOk = prevUnitProgress.Tasks.All(model => model.TestScore.IsOkProgress());
				if (!prevUnitIsOk)
					_logger.LogError("Can't start new unit, prev unit ({prev}) not finished completely for user {userId}", prevUnitIndex, userId);

				return prevUnitIsOk;
			}

			//If continue unit
			TaskEducationProgressGrpcModel progress = await GetTaskProgressAsync(userId, unitIndex, unit.Tasks[taskIndex - 1].Task);

			//Prev task must have progress
			bool progressHasProgress = progress?.HasProgress == true;
			if (!progressHasProgress)
				_logger.LogError("Invalid position while set task progress for user {userId}, prev task has no progress", userId);

			return progressHasProgress;
		}

		public async ValueTask<UnitStateGrpcModel> GetUnitProgressAsync(Guid? userId, int unit)
		{
			var result = new UnitStateGrpcModel();

			EducationProgressGrpcResponse progressResponse = await _progressService.GetProgressAsync(new GetEducationProgressGrpcRequest
			{
				Tutorial = TutorialHelper.Tutorial,
				Unit = unit,
				UserId = userId
			});

			int unitProgress = (progressResponse?.TaskScore).GetValueOrDefault();
			result.TestScore = unitProgress;

			if (unitProgress.IsMinProgress())
				return result;

			UnitEducationProgressGrpcResponse unitProgressResponse = await _progressService.GetUnitProgressAsync(new GetUnitEducationProgressGrpcRequest
			{
				Tutorial = TutorialHelper.Tutorial,
				Unit = unit,
				UserId = userId
			});

			TaskEducationProgressGrpcModel[] taskInfos = unitProgressResponse?.Progress;
			if (taskInfos == null)
				return result;

			var tasks = new List<TaskStateGrpcModel>();
			IDictionary<int, EducationStructureTask> unitTasks = EducationHelper.GetUnit(TutorialHelper.Tutorial, unit).Tasks;
			
			bool hasRetryCount = await _retryTaskService.HasRetryCountAsync(userId);
			DateTime? canRetryDate = null;

			async Task<bool> CanRetryByTime(DateTime? checkDate)
			{
				if (checkDate == null)
					return false;

				if (canRetryDate == null || checkDate < canRetryDate)
				{
					bool canRetry = await _retryTaskService.CanRetryByTimeAsync(userId, canRetryDate);
					if (canRetry)
						canRetryDate = checkDate;
					return canRetry;
				}

				return true;
			}

			foreach ((_, EducationStructureTask structureTask) in unitTasks)
			{
				int taskId = structureTask.Task;

				TaskEducationProgressGrpcModel taskProgress = taskInfos.FirstOrDefault(model => model.Task == taskId);
				if (taskProgress is not { HasProgress: true })
					break;

				int progressValue = taskProgress.Value;
				bool acceptByProgress = taskProgress.HasProgress && (!progressValue.IsMaxProgress() || structureTask.TaskType == EducationTaskType.Game);
				bool inRetryState = await _retryTaskService.TaskInRetryStateAsync(userId, unit, taskId);
				bool canRetryTask = !inRetryState && acceptByProgress;

				tasks.Add(new TaskStateGrpcModel
				{
					Task = taskId,
					TestScore = progressValue,
					RetryInfo = new TaskRetryInfoGrpcModel
					{
						InRetry = inRetryState,
						CanRetryByCount = canRetryTask && hasRetryCount,
						CanRetryByTime = canRetryTask && await CanRetryByTime(taskProgress.Date)
					}
				});
			}

			return new UnitStateGrpcModel
			{
				Unit = unit,
				TestScore = unitProgress,
				Tasks = tasks.ToArray()
			};
		}

		private async ValueTask<TaskEducationProgressGrpcModel> GetTaskProgressAsync(Guid? userId, int unit, int task)
		{
			TaskEducationProgressGrpcResponse taskProgressResponse = await _progressService.GetTaskProgressAsync(new GetTaskEducationProgressGrpcRequest
			{
				Tutorial = TutorialHelper.Tutorial,
				Unit = unit,
				Task = task,
				UserId = userId
			});

			return taskProgressResponse?.Progress;
		}

		public async ValueTask<TaskTypeProgressInfo> GetTotalProgressAsync(Guid? userId, int? unit = null)
		{
			TaskTypeProgressGrpcResponse typeProgressGrpcResponse = await _progressService.GetTaskTypeProgressAsync(new GetTaskTypeProgressGrpcRequest
			{
				Tutorial = TutorialHelper.Tutorial,
				Unit = unit,
				UserId = userId
			});

			var result = new TaskTypeProgressInfo();

			TaskTypeProgressGrpcModel[] typeProgressGrpcModels = typeProgressGrpcResponse?.TaskTypeProgress;
			if (typeProgressGrpcModels == null)
				return result;

			int CountByType(EducationTaskType taskType)
			{
				int[] values = typeProgressGrpcModels.First(model => model.TaskType == taskType).Values;

				return (int)Math.Round((double)values.Sum() / values.Length);
			}

			result.Text = CountByType(EducationTaskType.Text);
			result.Test = CountByType(EducationTaskType.Test);
			result.Case = CountByType(EducationTaskType.Case);
			result.Video = CountByType(EducationTaskType.Video);
			result.Game = CountByType(EducationTaskType.Game);
			result.TrueFalse = CountByType(EducationTaskType.TrueFalse);

			return result;
		}
	}
}