using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Service.Core.Domain.Models;
using Service.Core.Domain.Models.Education;
using Service.Core.Grpc.Models;
using Service.EducationProgress.Grpc;
using Service.EducationProgress.Grpc.Models;
using Service.EducationRetry.Grpc;
using Service.EducationRetry.Grpc.Models;
using Service.TutorialPersonal.Grpc.Models;
using Service.TutorialPersonal.Grpc.Models.State;

namespace Service.TutorialPersonal.Services
{
	public class TutorialHelperService : ITutorialHelperService
	{
		public const int MaxAnswerValuePrc = 100;
		public const int MinAnswerValuePrc = 0;

		private readonly IEducationProgressService _progressService;
		private readonly IEducationRetryService _retryService;
		private readonly ISystemClock _systemClock;

		public TutorialHelperService(IEducationProgressService progressService, IEducationRetryService retryService, ISystemClock systemClock)
		{
			_progressService = progressService;
			_retryService = retryService;
			_systemClock = systemClock;
		}

		public async ValueTask<TestScoreGrpcResponse> SetTaskProgressAsync(Guid? userId, EducationStructureUnit unit, EducationStructureTask task, bool isRetry, TimeSpan duration, float? progress = null)
		{
			bool canAnswer = await ValidatePostition(userId, unit, task);
			if (!canAnswer)
				return new TestScoreGrpcResponse {IsSuccess = false};

			TaskEducationProgressGrpcModel taskProgress = await GetTaskProgressAsync(userId, unit.Unit, task.Task);

			if (taskProgress is { HasProgress: false } && isRetry //retry without normal answered task
				|| taskProgress is { HasProgress: true, Value: MaxAnswerValuePrc } //retry 100% score task
				|| isRetry && !await GetRetryResultAsync(taskProgress, userId, unit)) //retry denied
				return new TestScoreGrpcResponse {IsSuccess = false};

			CommonGrpcResponse response = await _progressService.SetProgressAsync(new SetEducationProgressGrpcRequest
			{
				UserId = userId,
				Tutorial = EducationTutorial.PersonalFinance,
				Unit = unit.Unit,
				Task = task.Task,
				Value = progress ?? MaxAnswerValuePrc,
				Duration = duration
			});

			return new TestScoreGrpcResponse
			{
				IsSuccess = response.IsSuccess,
				Unit = await GetUnitProgressAsync(userId, unit)
			};
		}

		private async ValueTask<bool> ValidatePostition(Guid? userId, EducationStructureUnit unit, EducationStructureTask task)
		{
			if (unit.Unit == 1 && task.Task == 1)
				return true;

			if (unit.Unit > 1 && task.Task == 1)
			{
				unit = EducationStructure.Tutorials[EducationTutorial.PersonalFinance].Units[unit.Unit - 1];
				task = unit.Tasks[unit.Tasks.Values.Count - 1];
			}
			else
				task = unit.Tasks[task.Task - 1];

			TaskEducationProgressGrpcModel progress = await GetTaskProgressAsync(userId, unit.Unit, task.Task);

			return progress?.HasProgress == true;
		}

		public async ValueTask<PersonalStateUnitGrpcModel> GetUnitProgressAsync(Guid? userId, EducationStructureUnit unit)
		{
			int unitId = unit.Unit;

			EducationProgressGrpcResponse unitProgressResponse = await _progressService.GetProgressAsync(new GetEducationProgressGrpcRequest
			{
				Tutorial = EducationTutorial.PersonalFinance,
				Unit = unitId,
				UserId = userId
			});

			int unitProgress = (unitProgressResponse?.Progress.Value).GetValueOrDefault();
			if (unitProgress == 0)
				return null;

			var unitProgressItem = new PersonalStateUnitGrpcModel
			{
				Index = unitId,
				TestScore = unitProgress,
				Tasks = new List<PersonalStateTaskGrpcModel>()
			};

			foreach ((_, EducationStructureTask task) in unit.Tasks)
			{
				int taskId = task.Task;

				TaskEducationProgressGrpcModel taskProgress = await GetTaskProgressAsync(userId, unitId, taskId);
				if (!(taskProgress is { HasProgress: true }))
					break;

				unitProgressItem.Tasks.Add(new PersonalStateTaskGrpcModel
				{
					TaskId = taskId,
					TestScore = taskProgress.Value,
					CanRetry = CanRetryByTime(taskProgress) || await HasRetryCountAsync(userId)
				});
			}

			return unitProgressItem;
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

		public async ValueTask<bool> HasRetryCountAsync(Guid? userId)
		{
			RetryCountGrpcResponse retryResponse = await _retryService.GetRetryCountAsync(new GetRetryCountGrpcRequest
			{
				UserId = userId
			});

			return retryResponse?.Count > 0;
		}

		public bool CanRetryByTime(TaskEducationProgressGrpcModel progressGrpcModel) => _systemClock.Now.Subtract(progressGrpcModel.WhenFinished).TotalDays >= 1;

		public static float CheckAnswer(float progress, PersonalTaskTestAnswerGrpcModel[] answers, int questionNumber, params int[] answerNumbers)
		{
			PersonalTaskTestAnswerGrpcModel answer = answers.FirstOrDefault(model => model.Number == questionNumber);

			return answer != null && answerNumbers.Intersect(answer.Value).Count() == answerNumbers.Length
				? progress
				: MinAnswerValuePrc;
		}

		public static float CheckAnswer(float progress, PersonalTaskTrueFalseAnswerGrpcModel[] answers, int questionNumber, bool answerValue)
		{
			PersonalTaskTrueFalseAnswerGrpcModel answer = answers.FirstOrDefault(model => model.Number == questionNumber);

			return answer != null && answer.Value == answerValue
				? progress
				: MinAnswerValuePrc;
		}
	}
}