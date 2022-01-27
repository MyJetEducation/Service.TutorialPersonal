using Service.TutorialPersonal.Grpc.Models.State;
using Service.UserProgress.Grpc.Models;

namespace Service.TutorialPersonal.Mappers
{
	public static class ProgressInfoMapper
	{
		public static ProgressItemInfoGrpcModel ToGrpcModel(this ProgressGrpcResponse response) => new ProgressItemInfoGrpcModel
		{
			Index = response.Index,
			Count = response.Count,
			Progress = response.Progress
		};
	}
}