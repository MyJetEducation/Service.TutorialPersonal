using System;
using System.Threading.Tasks;

namespace Service.TutorialPersonal.Services
{
	public interface IRetryTaskService
	{
		ValueTask<bool> TaskInRetryStateAsync(Guid? userId, int unit, int task);

		bool CanRetryByTimeAsync(DateTime? progressDate, DateTime? lastRetryDate);

		ValueTask<bool> HasRetryCountAsync(Guid? userId);

		ValueTask<bool> ClearTaskRetryStateAsync(Guid? userId, int unit, int task);

		ValueTask<DateTime?> GetRetryLastDateAsync(Guid? userId);
	}
}