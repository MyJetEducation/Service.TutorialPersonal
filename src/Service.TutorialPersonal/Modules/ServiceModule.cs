using Autofac;
using Microsoft.Extensions.Logging;
using Service.EducationProgress.Client;
using Service.EducationRetry.Client;
using Service.UserProgress.Client;
using Service.UserReward.Client;

namespace Service.TutorialPersonal.Modules
{
	public class ServiceModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterEducationRetryClient(Program.Settings.EducationRetryServiceUrl, Program.LogFactory.CreateLogger(typeof(EducationRetryClientFactory)));

			builder.RegisterEducationProgressClient(Program.Settings.EducationProgressServiceUrl);
			builder.RegisterUserRewardClient(Program.Settings.UserRewardServiceUrl);
			builder.RegisterUserProgressClient(Program.Settings.UserProgressServiceUrl);
		}
	}
}