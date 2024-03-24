using UnityEngine;

namespace Scripts.Distance
{
	[CreateAssetMenu( fileName = "DistanceConfig", menuName = "Configs/DistanceConfig" )]
	public class DistanceConfig : ScriptableObject
	{
		public float maxDifficultyDistance = 100;
		public float newDistanceTolerance  = 0.05f;
	}
}