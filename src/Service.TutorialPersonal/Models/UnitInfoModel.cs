using Service.TutorialPersonal.Grpc.Models.State;

namespace Service.TutorialPersonal.Models
{
	public class UnitInfoModel
	{
		public PersonalStateUnitGrpcModel PersonalStateUnit { get; set; }

		public int TrueFalseProgress { get; set; }

		public int CaseProgress { get; set; }
	}
}