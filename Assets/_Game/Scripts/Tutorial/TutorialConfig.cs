using UnityEngine;

namespace Scripts.Tutorial
{
	[CreateAssetMenu( fileName = "TutorialConfig", menuName = "Configs/TutorialConfig" )]
	public class TutorialConfig : ScriptableObject
	{
		public float waitToShowTutorial = 0.4f;
		public float pulseScale         = 1.2f;
		public float pulseDuration      = 0.1f;
		public int   passMoveCount      = 3;
		public float longMoveDistance   = 4;
	}
}