using System.Threading.Tasks;
using Service.Core.Client.Education;
using Service.TutorialPersonal.Grpc.Models;
using Service.TutorialPersonal.Grpc.Models.State;
using static Service.TutorialPersonal.Services.AnswerHelper;

namespace Service.TutorialPersonal.Services
{
	public partial class TutorialPersonalService
	{
		public static readonly EducationStructureUnit Unit2 = Tutorial.Units[2];

		public async ValueTask<TestScoreGrpcResponse> Unit2TextAsync(PersonalTaskTextGrpcRequest request) => 
			await _taskProgressService.SetTaskProgressAsync(request.UserId, Unit2, Unit2.Tasks[1], request.IsRetry, request.Duration);

		public async ValueTask<TestScoreGrpcResponse> Unit2TestAsync(PersonalTaskTestGrpcRequest request)
		{
			PersonalTaskTestAnswerGrpcModel[] answers = request.Answers;

			int progress = CheckAnswer(20, answers, 1, 1, 2, 3)
				+ CheckAnswer(20, answers, 2, 1, 2)
				+ CheckAnswer(20, answers, 3, 2)
				+ CheckAnswer(20, answers, 4, 1, 2, 3)
				+ CheckAnswer(20, answers, 5, 1, 2);

			return await _taskProgressService.SetTaskProgressAsync(request.UserId, Unit2, Unit2.Tasks[2], request.IsRetry, request.Duration, progress);
		}

		public async ValueTask<TestScoreGrpcResponse> Unit2VideoAsync(PersonalTaskVideoGrpcRequest request) => 
			await _taskProgressService.SetTaskProgressAsync(request.UserId, Unit2, Unit2.Tasks[3], request.IsRetry, request.Duration);

		public async ValueTask<TestScoreGrpcResponse> Unit2CaseAsync(PersonalTaskCaseGrpcRequest request) => 
			await _taskProgressService.SetTaskProgressAsync(request.UserId, Unit2, Unit2.Tasks[4], request.IsRetry, request.Duration, GetSimpleProgress(request.Value == 1));

		public async ValueTask<TestScoreGrpcResponse> Unit2TrueFalseAsync(PersonalTaskTrueFalseGrpcRequest request)
		{
			PersonalTaskTrueFalseAnswerGrpcModel[] answers = request.Answers;

			int progress = CheckAnswer(20, answers, 1, false)
				+ CheckAnswer(20, answers, 2, false)
				+ CheckAnswer(20, answers, 3, true)
				+ CheckAnswer(20, answers, 4, false)
				+ CheckAnswer(20, answers, 5, true);

			return await _taskProgressService.SetTaskProgressAsync(request.UserId, Unit2, Unit2.Tasks[5], request.IsRetry, request.Duration, progress);
		}

		public async ValueTask<TestScoreGrpcResponse> Unit2GameAsync(PersonalTaskGameGrpcRequest request) => 
			await _taskProgressService.SetTaskProgressAsync(request.UserId, Unit2, Unit2.Tasks[6], request.IsRetry, request.Duration, GetSimpleProgress(request.Passed));
	}
}