using System.Threading.Tasks;
using Service.Education;
using Service.Education.Structure;
using Service.TutorialPersonal.Grpc.Models;
using Service.TutorialPersonal.Grpc.Models.State;
using static Service.Education.Helpers.AnswerHelper;

namespace Service.TutorialPersonal.Services
{
	public partial class TutorialPersonalService
	{
		public static readonly EducationStructureUnit Unit5 = EducationStructure.Tutorials[EducationTutorial.PersonalFinance].Units[4];

		public async ValueTask<TestScoreGrpcResponse> Unit5TextAsync(PersonalTaskTextGrpcRequest request)
			=> await _taskProgressService.SetTaskProgressAsync(request.UserId, Unit5, Unit5.Tasks[1], request.IsRetry, request.Duration);

		public async ValueTask<TestScoreGrpcResponse> Unit5TestAsync(PersonalTaskTestGrpcRequest request)
		{
			ITaskTestAnswer[] answers = request.Answers;

			int progress = CheckAnswer(20, answers, 1, 2, 3)
				+ CheckAnswer(20, answers, 2, 1, 2)
				+ CheckAnswer(20, answers, 3, 3)
				+ CheckAnswer(20, answers, 4, 2)
				+ CheckAnswer(20, answers, 5, 2);

			return await _taskProgressService.SetTaskProgressAsync(request.UserId, Unit5, Unit5.Tasks[2], request.IsRetry, request.Duration, progress);
		}

		public async ValueTask<TestScoreGrpcResponse> Unit5VideoAsync(PersonalTaskVideoGrpcRequest request) => 
			await _taskProgressService.SetTaskProgressAsync(request.UserId, Unit5, Unit5.Tasks[3], request.IsRetry, request.Duration);

		public async ValueTask<TestScoreGrpcResponse> Unit5CaseAsync(PersonalTaskCaseGrpcRequest request) => 
			await _taskProgressService.SetTaskProgressAsync(request.UserId, Unit5, Unit5.Tasks[4], request.IsRetry, request.Duration, CountProgress(request.Value == 1));

		public async ValueTask<TestScoreGrpcResponse> Unit5TrueFalseAsync(PersonalTaskTrueFalseGrpcRequest request)
		{
			ITaskTrueFalseAnswer[] answers = request.Answers;

			int progress = CheckAnswer(20, answers, 1, false)
				+ CheckAnswer(20, answers, 2, false)
				+ CheckAnswer(20, answers, 3, true)
				+ CheckAnswer(20, answers, 4, true)
				+ CheckAnswer(20, answers, 5, true);

			return await _taskProgressService.SetTaskProgressAsync(request.UserId, Unit5, Unit5.Tasks[5], request.IsRetry, request.Duration, progress);
		}

		public async ValueTask<TestScoreGrpcResponse> Unit5GameAsync(PersonalTaskGameGrpcRequest request) => 
			await _taskProgressService.SetTaskProgressAsync(request.UserId, Unit5, Unit5.Tasks[6], request.IsRetry, request.Duration);
	}
}