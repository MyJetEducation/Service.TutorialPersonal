using System.ServiceModel;
using System.Threading.Tasks;
using Service.TutorialPersonal.Grpc.Models.State;
using Service.TutorialPersonal.Grpc.Models.Task;

namespace Service.TutorialPersonal.Grpc
{
	[ServiceContract]
	public interface ITutorialPersonalService
	{
		[OperationContract]
		ValueTask<FinishStateGrpcResponse> GetFinishStateAsync(GetFinishStateGrpcRequest request);

		#region Unit1

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit1TextAsync(TaskTextGrpcRequest request);

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit1TestAsync(TaskTestGrpcRequest request);

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit1VideoAsync(TaskVideoGrpcRequest request);

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit1CaseAsync(TaskCaseGrpcRequest request);

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit1TrueFalseAsync(TaskTrueFalseGrpcRequest request);

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit1GameAsync(TaskGameGrpcRequest request);

		#endregion

		#region Unit2

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit2TextAsync(TaskTextGrpcRequest request);

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit2TestAsync(TaskTestGrpcRequest request);

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit2VideoAsync(TaskVideoGrpcRequest request);

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit2CaseAsync(TaskCaseGrpcRequest request);

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit2TrueFalseAsync(TaskTrueFalseGrpcRequest request);

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit2GameAsync(TaskGameGrpcRequest request);

		#endregion

		#region Unit3

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit3TextAsync(TaskTextGrpcRequest request);

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit3TestAsync(TaskTestGrpcRequest request);

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit3VideoAsync(TaskVideoGrpcRequest request);

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit3CaseAsync(TaskCaseGrpcRequest request);

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit3TrueFalseAsync(TaskTrueFalseGrpcRequest request);

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit3GameAsync(TaskGameGrpcRequest request);

		#endregion

		#region Unit4

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit4TextAsync(TaskTextGrpcRequest request);

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit4TestAsync(TaskTestGrpcRequest request);

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit4VideoAsync(TaskVideoGrpcRequest request);

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit4CaseAsync(TaskCaseGrpcRequest request);

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit4TrueFalseAsync(TaskTrueFalseGrpcRequest request);

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit4GameAsync(TaskGameGrpcRequest request);

		#endregion

		#region Unit5

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit5TextAsync(TaskTextGrpcRequest request);

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit5TestAsync(TaskTestGrpcRequest request);

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit5VideoAsync(TaskVideoGrpcRequest request);

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit5CaseAsync(TaskCaseGrpcRequest request);

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit5TrueFalseAsync(TaskTrueFalseGrpcRequest request);

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit5GameAsync(TaskGameGrpcRequest request);

		#endregion
	}
}