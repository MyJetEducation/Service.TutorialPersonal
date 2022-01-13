using System;
using System.Threading.Tasks;
using Service.EducationProgress.Grpc.Models;

namespace Service.TutorialPersonal.Services
{
	public interface IRetryTaskService
	{
		ValueTask<bool> TaskInRetryStateAsync(Guid? userId, int unit, int task);

		ValueTask<bool> CanRetryByTimeAsync(Guid? userId, TaskEducationProgressGrpcModel progressGrpcModel);

		ValueTask<bool> HasRetryCountAsync(Guid? userId);

		ValueTask<bool> ClearTaskRetryStateAsync(Guid? userId, int unit, int task);
	}
}