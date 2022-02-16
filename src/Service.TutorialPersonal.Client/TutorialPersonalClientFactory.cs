using JetBrains.Annotations;
using MyJetWallet.Sdk.Grpc;
using Service.TutorialPersonal.Grpc;

namespace Service.TutorialPersonal.Client
{
	[UsedImplicitly]
	public class TutorialPersonalClientFactory : MyGrpcClientFactory
	{
		public TutorialPersonalClientFactory(string grpcServiceUrl) : base(grpcServiceUrl)
		{
		}

		public ITutorialPersonalService GetTutorialPersonalService() => CreateGrpcService<ITutorialPersonalService>();
	}
}
