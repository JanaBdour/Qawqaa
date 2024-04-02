using UnityEngine;

namespace Scripts.FallingSteps
{
	[CreateAssetMenu( fileName = "FallingStepsConfig", menuName = "Configs/FallingStepsConfig" )]
	public class FallingStepsConfig : ScriptableObject
	{
		public FallingStepView prefab;

		[Range( 0, 1 )] public float minDifficulty = 0.3f;
		[Range( 0, 1 )] public float probability   = 0.6f;

		public float yOffset                     = -0.1f;
		public float stepDistance                = 0.5f;
		public float minDistanceBetweenPlatforms = 7;
		public int   maxMissingSteps             = 5;
		public float releaseWait                 = 2;
		public int   poolInitialCount            = 30;
	}
}