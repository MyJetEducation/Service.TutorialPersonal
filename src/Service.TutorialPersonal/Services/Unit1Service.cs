using System.Threading.Tasks;
using Service.Education;
using Service.Education.Structure;
using Service.TutorialPersonal.Grpc.Models.Task;
using Service.TutorialPersonal.Helper;
using static Service.Education.Helpers.AnswerHelper;

namespace Service.TutorialPersonal.Services
{
	public partial class TutorialPersonalService
	{
		private static readonly EducationStructureUnit Unit1 = TutorialHelper.StructureTutorial.Units[1];

		public async ValueTask<TestScoreGrpcResponse> Unit1TextAsync(TaskTextGrpcRequest request) =>
			await _taskProgressService.SetTaskProgressAsync(request.UserId, Unit1, Unit1.Tasks[1], request.IsRetry, request.Duration);

		public async ValueTask<TestScoreGrpcResponse> Unit1TestAsync(TaskTestGrpcRequest request)
		{
			ITaskTestAnswer[] answers = request.Answers;

			int progress = CheckAnswer(20, answers, 1, 1, 3)
				+ CheckAnswer(20, answers, 2, 1, 3)
				+ CheckAnswer(20, answers, 3, 3)
				+ CheckAnswer(20, answers, 4, 1, 2, 3)
				+ CheckAnswer(20, answers, 5, 1, 3);

			return await _taskProgressService.SetTaskProgressAsync(request.UserId, Unit1, Unit1.Tasks[2], request.IsRetry, request.Duration, progress);
		}

		public async ValueTask<TestScoreGrpcResponse> Unit1VideoAsync(TaskVideoGrpcRequest request) =>
			await _taskProgressService.SetTaskProgressAsync(request.UserId, Unit1, Unit1.Tasks[3], request.IsRetry, request.Duration);

		public async ValueTask<TestScoreGrpcResponse> Unit1CaseAsync(TaskCaseGrpcRequest request) =>
			await _taskProgressService.SetTaskProgressAsync(request.UserId, Unit1, Unit1.Tasks[4], request.IsRetry, request.Duration, CountProgress(request.Value == 1));

		public async ValueTask<TestScoreGrpcResponse> Unit1TrueFalseAsync(TaskTrueFalseGrpcRequest request)
		{
			ITaskTrueFalseAnswer[] answers = request.Answers;

			int progress = CheckAnswer(20, answers, 1, true)
				+ CheckAnswer(20, answers, 2, true)
				+ CheckAnswer(20, answers, 3, true)
				+ CheckAnswer(20, answers, 4, false)
				+ CheckAnswer(20, answers, 5, true);

			return await _taskProgressService.SetTaskProgressAsync(request.UserId, Unit1, Unit1.Tasks[5], request.IsRetry, request.Duration, progress);
		}

		public async ValueTask<TestScoreGrpcResponse> Unit1GameAsync(TaskGameGrpcRequest request) =>
			await _taskProgressService.SetTaskProgressAsync(request.UserId, Unit1, Unit1.Tasks[6], request.IsRetry, request.Duration);
	}
}