using Autofac;
using Service.Core.Domain.Models;
using Service.TutorialPersonal.Services;

namespace Service.TutorialPersonal.Modules
{
    public class SettingsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(Program.Settings).AsSelf().SingleInstance();
	        builder.RegisterType<TutorialHelperService>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<ISystemClock>().AsImplementedInterfaces().SingleInstance();
        }
    }
}
