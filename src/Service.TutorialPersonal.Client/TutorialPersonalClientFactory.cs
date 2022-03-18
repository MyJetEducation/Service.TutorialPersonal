using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Service.Grpc;
using Service.TutorialPersonal.Grpc;

namespace Service.TutorialPersonal.Client
{
	[UsedImplicitly]
	public class TutorialPersonalClientFactory : GrpcClientFactory
	{
		public TutorialPersonalClientFactory(string grpcServiceUrl, ILogger logger) : base(grpcServiceUrl, logger)
		{
		}

		public IGrpcServiceProxy<ITutorialPersonalService> GetTutorialPersonalService() => CreateGrpcService<ITutorialPersonalService>();
	}
}