using System;
using System.Threading.Tasks;
using Service.Core.Domain.Models.Education;
using Service.EducationProgress.Grpc.Models;
using Service.TutorialPersonal.Grpc.Models.State;

namespace Service.TutorialPersonal.Services
{
	public interface ITutorialHelperService
	{
		ValueTask<TestScoreGrpcResponse> SetTaskProgressAsync(Guid? userId, EducationStructureUnit unit, EducationStructureTask task, bool isRetry, TimeSpan duration, float? progress = null);

		ValueTask<PersonalStateUnitGrpcModel> GetUnitProgressAsync(Guid? userId, EducationStructureUnit unit);

		ValueTask<TaskEducationProgressGrpcModel> GetTaskProgressAsync(Guid? userId, int unit, int task);

		ValueTask<bool> HasRetryCountAsync(Guid? userId);

		ValueTask<bool> GetRetryResultAsync(TaskEducationProgressGrpcModel taskProgress, Guid? userId, EducationStructureUnit unit);

		bool CanRetryByTime(TaskEducationProgressGrpcModel progressGrpcModel);
	}
}