using Scripts.Effects;
using UnityEngine;

namespace Scripts.Collectible
{
	[CreateAssetMenu( fileName = "CollectibleConfig", menuName = "Configs/CollectibleConfig" )]
	public class CollectibleConfig : ScriptableObject
	{
		public CollectibleView prefab;

		[Header( "Spawn Settings" )]
		public Vector3 offset              = new Vector3( 0, 0.5f );
		public int     minCountPerPlatform = 0;
		public int     maxCountPerPlatform = 3;

		[Header( "Collect Settings" )]
		public EffectView collectEffectPrefab;
		public float      collectDistance = 1;

		[Header( "Animation Settings" )]
		public float minAnimationSpeed    = 1;
		public float maxAnimationSpeed    = 3;
		public float minAnimationDistance = -1;
		public float maxAnimationDistance = 3;
	}
}