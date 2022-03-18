using Autofac;
using Microsoft.Extensions.Logging;
using Service.Grpc;
using Service.TutorialPersonal.Grpc;

// ReSharper disable UnusedMember.Global

namespace Service.TutorialPersonal.Client
{
	public static class AutofacHelper
	{
		public static void RegisterTutorialPersonalClient(this ContainerBuilder builder, string grpcServiceUrl, ILogger logger)
		{
			var factory = new TutorialPersonalClientFactory(grpcServiceUrl, logger);

			builder.RegisterInstance(factory.GetTutorialPersonalService()).As<IGrpcServiceProxy<ITutorialPersonalService>>().SingleInstance();
		}
	}
}
