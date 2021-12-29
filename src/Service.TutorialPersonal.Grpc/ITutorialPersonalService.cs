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

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit1TextAsync(PersonalTaskTextGrpcRequest request);

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit1TestAsync(PersonalTaskTestGrpcRequest request);

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit1CaseAsync(PersonalTaskCaseGrpcRequest request);

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit1TrueFalseAsync(PersonalTaskTrueFalseGrpcRequest request);

		[OperationContract]
		ValueTask<TestScoreGrpcResponse> Unit1GameAsync(PersonalTaskGameGrpcRequest request);
	}
}