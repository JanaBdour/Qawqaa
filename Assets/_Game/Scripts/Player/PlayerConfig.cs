using UnityEngine;

namespace Scripts.Player
{
	[CreateAssetMenu( fileName = "PlayerConfig", menuName = "Configs/PlayerConfig" )]
	public class PlayerConfig : ScriptableObject
	{
		public PlayerView prefab;
		public Vector3    position;
		public Quaternion rotation;
		public float      platformDistance  = 0.05f;
		public float      trailEnableDelay  = 0.1f;
		public int        maxJumpCount      = 3;
		public Vector2    throwForce        = new Vector2( 200, 130 );
		public LayerMask  platformLayerMask;
	}
}