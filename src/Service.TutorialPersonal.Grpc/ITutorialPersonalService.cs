using System.ServiceModel;
using System.Threading.Tasks;
using Service.TutorialPersonal.Grpc.Models;
using Service.TutorialPersonal.Grpc.Models.State;

namespace Service.TutorialPersonal.Grpc
{
	[ServiceContract]
	public interface ITutorialPersonalService
	{
		[OperationContract]
		ValueTask<PersonalStateGrpcResponse> GetDashboardStateAsync(PersonalSelectTaskUnitGrpcRequest request);

		[OperationContract]
		ValueTask<FinishUnitGrpcResponse> GetFinishStateAsync(GetFinishStateGrpcRequest request);

		#region Unit1

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit1TextAsync(PersonalTaskTextGrpcRequest request);

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit1TestAsync(PersonalTaskTestGrpcRequest request);

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit1VideoAsync(PersonalTaskVideoGrpcRequest request);

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit1CaseAsync(PersonalTaskCaseGrpcRequest request);

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit1TrueFalseAsync(PersonalTaskTrueFalseGrpcRequest request);

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit1GameAsync(PersonalTaskGameGrpcRequest request);

		#endregion

		#region Unit2

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit2TextAsync(PersonalTaskTextGrpcRequest request);

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit2TestAsync(PersonalTaskTestGrpcRequest request);

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit2VideoAsync(PersonalTaskVideoGrpcRequest request);

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit2CaseAsync(PersonalTaskCaseGrpcRequest request);

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit2TrueFalseAsync(PersonalTaskTrueFalseGrpcRequest request);

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit2GameAsync(PersonalTaskGameGrpcRequest request);

		#endregion

		#region Unit3

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit3TextAsync(PersonalTaskTextGrpcRequest request);

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit3TestAsync(PersonalTaskTestGrpcRequest request);

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit3VideoAsync(PersonalTaskVideoGrpcRequest request);

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit3CaseAsync(PersonalTaskCaseGrpcRequest request);

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit3TrueFalseAsync(PersonalTaskTrueFalseGrpcRequest request);

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit3GameAsync(PersonalTaskGameGrpcRequest request);

		#endregion

		#region Unit4

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit4TextAsync(PersonalTaskTextGrpcRequest request);

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit4TestAsync(PersonalTaskTestGrpcRequest request);

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit4VideoAsync(PersonalTaskVideoGrpcRequest request);

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit4CaseAsync(PersonalTaskCaseGrpcRequest request);

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit4TrueFalseAsync(PersonalTaskTrueFalseGrpcRequest request);

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit4GameAsync(PersonalTaskGameGrpcRequest request);

		#endregion

		#region Unit5

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit5TextAsync(PersonalTaskTextGrpcRequest request);

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit5TestAsync(PersonalTaskTestGrpcRequest request);

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit5VideoAsync(PersonalTaskVideoGrpcRequest request);

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit5CaseAsync(PersonalTaskCaseGrpcRequest request);

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit5TrueFalseAsync(PersonalTaskTrueFalseGrpcRequest request);

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit5GameAsync(PersonalTaskGameGrpcRequest request);

		#endregion
	}
}