using UnityEngine;

namespace Scripts.Platforms
{
	[CreateAssetMenu( fileName = "PlatformsConfig", menuName = "Configs/PlatformsConfig" )]
	public class PlatformsConfig : ScriptableObject
	{
		public PlatformView   startPlatformPrefab;
		public PlatformView[] platformPrefabs;
		public Quaternion     rotation;
		public int            startCount        = 15;
		public int            poolInitialCount  = 20;
		public Vector3        minDistance       = new Vector3( 2, -2 );
		public Vector3        maxDistance       = new Vector3( 8, 2 );
		public float          disappearDistance = 10;
	}
}