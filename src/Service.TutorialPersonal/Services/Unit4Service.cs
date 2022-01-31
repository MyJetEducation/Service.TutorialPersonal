using System.Threading.Tasks;
using Service.Core.Client.Education;
using Service.TutorialPersonal.Grpc.Models;
using Service.TutorialPersonal.Grpc.Models.State;
using static Service.TutorialPersonal.Services.AnswerHelper;

namespace Service.TutorialPersonal.Services
{
	public partial class TutorialPersonalService
	{
		public static readonly EducationStructureUnit Unit4 = Tutorial.Units[4];

		public async ValueTask<TestScoreGrpcResponse> Unit4TextAsync(PersonalTaskTextGrpcRequest request)
		{
			return await _taskProgressService.SetTaskProgressAsync(request.UserId, Unit4, Unit4.Tasks[1], request.IsRetry, request.Duration);
		}

		public async ValueTask<TestScoreGrpcResponse> Unit4TestAsync(PersonalTaskTestGrpcRequest request)
		{
			PersonalTaskTestAnswerGrpcModel[] answers = request.Answers;

			int progress = CheckAnswer(20, answers, 1, 2, 3)
				+ CheckAnswer(20, answers, 2, 1)
				+ CheckAnswer(20, answers, 3, 1, 3)
				+ CheckAnswer(20, answers, 4, 2)
				+ CheckAnswer(20, answers, 5, 2);

			return await _taskProgressService.SetTaskProgressAsync(request.UserId, Unit4, Unit4.Tasks[2], request.IsRetry, request.Duration, progress);
		}

		public async ValueTask<TestScoreGrpcResponse> Unit4CaseAsync(PersonalTaskCaseGrpcRequest request)
		{
			int progress = request.Value == 1
				? MaxAnswerProgress
				: MinAnswerProgress;

			return await _taskProgressService.SetTaskProgressAsync(request.UserId, Unit4, Unit4.Tasks[3], request.IsRetry, request.Duration, progress);
		}

		public async ValueTask<TestScoreGrpcResponse> Unit4TrueFalseAsync(PersonalTaskTrueFalseGrpcRequest request)
		{
			PersonalTaskTrueFalseAnswerGrpcModel[] answers = request.Answers;

			int progress = CheckAnswer(20, answers, 1, false)
				+ CheckAnswer(20, answers, 2, true)
				+ CheckAnswer(20, answers, 3, true)
				+ CheckAnswer(20, answers, 4, false)
				+ CheckAnswer(20, answers, 5, false);

			return await _taskProgressService.SetTaskProgressAsync(request.UserId, Unit4, Unit4.Tasks[4], request.IsRetry, request.Duration, progress);
		}

		public async ValueTask<TestScoreGrpcResponse> Unit4GameAsync(PersonalTaskGameGrpcRequest request)
		{
			return await _taskProgressService.SetTaskProgressAsync(request.UserId, Unit4, Unit4.Tasks[5], request.IsRetry, request.Duration);
		}
	}
}