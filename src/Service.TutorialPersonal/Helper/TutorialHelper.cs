using Service.Education.Structure;

namespace Service.TutorialPersonal.Helper
{
	public static class TutorialHelper
	{
		public static readonly EducationTutorial Tutorial = EducationTutorial.PersonalFinance;

		public static readonly EducationStructureTutorial EducationStructureTutorial = EducationStructure.Tutorials[Tutorial];
	}
}