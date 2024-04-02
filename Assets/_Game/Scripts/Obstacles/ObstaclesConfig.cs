using UnityEngine;

namespace Scripts.Obstacles
{
	[CreateAssetMenu( fileName = "ObstaclesConfig", menuName = "Configs/ObstaclesConfig" )]
	public class ObstaclesConfig : ScriptableObject
	{
		public ObstacleView[] prefabs;
		public float          xDistance        = 2f;
		public float          minDistance      = 0.4f;
		public float          maxDistance      = 0.8f;
		public float          borderSize       = 0.3f;
		public int            poolInitialCount = 50;

		[Range( 0, 1 )] public float probability = 0.7f;
	}
}