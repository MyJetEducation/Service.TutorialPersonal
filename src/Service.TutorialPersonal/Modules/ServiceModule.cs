using Autofac;
using Service.EducationPosition.Client;
using Service.EducationProgress.Client;
using Service.EducationRetry.Client;
using Service.UserKnowledge.Client;

namespace Service.TutorialPersonal.Modules
{
	public class ServiceModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterUserKnowledgeClient(Program.Settings.UserKnowledgeServiceUrl);
			builder.RegisterEducationProgressClient(Program.Settings.EducationProgressServiceUrl);
			builder.RegisterEducationPositionClient(Program.Settings.EducationPositionServiceUrl);
			builder.RegisterEducationRetryClient(Program.Settings.EducationRetryServiceUrl);
		}
	}
}