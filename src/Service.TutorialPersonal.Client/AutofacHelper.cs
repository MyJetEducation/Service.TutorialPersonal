using Autofac;
using Service.TutorialPersonal.Grpc;

// ReSharper disable UnusedMember.Global

namespace Service.TutorialPersonal.Client
{
    public static class AutofacHelper
    {
        public static void RegisterTutorialPersonalClient(this ContainerBuilder builder, string grpcServiceUrl)
        {
            var factory = new TutorialPersonalClientFactory(grpcServiceUrl);

            builder.RegisterInstance(factory.GetTutorialPersonalService()).As<ITutorialPersonalService>().SingleInstance();
        }
    }
}
