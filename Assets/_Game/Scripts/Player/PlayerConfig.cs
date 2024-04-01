using UnityEngine;

namespace Scripts.Player
{
	[CreateAssetMenu( fileName = "PlayerConfig", menuName = "Configs/PlayerConfig" )]
	public class PlayerConfig : ScriptableObject
	{
		public PlayerView prefab;
		public Vector3    position;
		public Quaternion rotation;

		public float trailEnableDelay = 0.1f;
		
		[Header( "Combo Settings" )]
		public float fresnelFadeDuration      = 0.5f;
		public float comboRotatingSpeed       = 5;
		public float comboResettingSpeed      = 3;
		public float comboObstacleForceRadius = 5;
		public float comboObstacleForce       = 100;
		
		[Header( "Movement Settings" )]
		public int       maxJumpCount       = 3;
		public Vector2   throwForce         = new Vector2( 200, 130 );
		public float     platformDistance   = 0.05f;
		public LayerMask platformLayerMask;
		
		public float tapDuration        = 0.01f;
		public float holdDuration       = 1;
		public float longMoveMultiplier = 2;

	}
}