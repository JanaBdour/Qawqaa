using Scripts.Effects;
using UnityEngine;

namespace Scripts.Projectiles
{
	[CreateAssetMenu( fileName = "ProjectilesConfig", menuName = "Configs/ProjectilesConfig" )]
	public class ProjectilesConfig : ScriptableObject
	{
		public ProjectileView prefab;
		public EffectView     effectPrefab;
		public Vector2        force           = new Vector2( 600, 100 );
		public float          torque          = 0.3f;
		public float          forceMultiplier = 1.3f;
		public float          maxLifetime     = 2;
		public float          normalScale     = 1;
		public float          pulseAddedScale = 0.5f;
		public float          pulseSpeed      = 4;
	}
}