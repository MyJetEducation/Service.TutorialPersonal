using System;
using System.Threading.Tasks;
using Service.Education.Structure;
using Service.TutorialPersonal.Grpc.Models.State;
using Service.TutorialPersonal.Grpc.Models.Task;
using Service.TutorialPersonal.Models;

namespace Service.TutorialPersonal.Services
{
	public interface ITaskProgressService
	{
		ValueTask<TestScoreGrpcResponse> SetTaskProgressAsync(string userId, EducationStructureUnit unit, EducationStructureTask task, bool isRetry, TimeSpan duration, int? progress = null);

		ValueTask<UnitStateGrpcModel> GetUnitProgressAsync(string userId, int unit);

		ValueTask<TaskTypeProgressInfo> GetTotalProgressAsync(string userId, int? unit = null);
	}
}