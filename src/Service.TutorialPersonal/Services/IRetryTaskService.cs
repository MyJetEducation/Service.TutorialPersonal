using System;
using System.Threading.Tasks;

namespace Service.TutorialPersonal.Services
{
	public interface IRetryTaskService
	{
		ValueTask<bool> TaskInRetryStateAsync(string userId, int unit, int task);

		bool CanRetryByTimeAsync(DateTime? progressDate, DateTime? lastRetryDate);

		ValueTask<bool> HasRetryCountAsync(string userId);

		ValueTask<bool> ClearTaskRetryStateAsync(string userId, int unit, int task);

		ValueTask<DateTime?> GetRetryLastDateAsync(string userId);
	}
}