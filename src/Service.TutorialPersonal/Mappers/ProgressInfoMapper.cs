using Service.Core.Client.Constants;
using Service.Education.Contracts.State;
using Service.TutorialPersonal.Models;

namespace Service.TutorialPersonal.Mappers
{
	public static class ProgressInfoMapper
	{
		public static FinishStateGrpcResponse ToGrpcModel(this TaskTypeProgressInfo info, UserAchievement[] achievements) => new()
		{
			Case = info.Case,
			TrueFalse = info.TrueFalse,
			Game = info.Game,
			Test = info.Test,
			Text = info.Text,
			Video = info.Video,
			Achievements = achievements
		};
	}
}