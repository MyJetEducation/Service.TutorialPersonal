using Autofac;
using Service.EducationPosition.Client;
using Service.EducationProgress.Client;
using Service.EducationRetry.Client;

namespace Service.TutorialPersonal.Modules
{
	public class ServiceModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterEducationProgressClient(Program.Settings.EducationProgressServiceUrl);
			builder.RegisterEducationPositionClient(Program.Settings.EducationPositionServiceUrl);
			builder.RegisterEducationRetryClient(Program.Settings.EducationRetryServiceUrl);
		}
	}
}