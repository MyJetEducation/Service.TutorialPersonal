using MyJetWallet.Sdk.Service;
using MyYamlParser;

namespace Service.TutorialPersonal.Settings
{
	public class SettingsModel
	{
		[YamlProperty("TutorialPersonal.SeqServiceUrl")]
		public string SeqServiceUrl { get; set; }

		[YamlProperty("TutorialPersonal.ZipkinUrl")]
		public string ZipkinUrl { get; set; }

		[YamlProperty("TutorialPersonal.ElkLogs")]
		public LogElkSettings ElkLogs { get; set; }

		[YamlProperty("TutorialPersonal.EducationPositionServiceUrl")]
		public string EducationPositionServiceUrl { get; set; }

		[YamlProperty("TutorialPersonal.EducationProgressServiceUrl")]
		public string EducationProgressServiceUrl { get; set; }

		[YamlProperty("TutorialPersonal.EducationRetryServiceUrl")]
		public string EducationRetryServiceUrl { get; set; }
	}
}