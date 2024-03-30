using UnityEngine;

namespace Scripts.Player
{
	[CreateAssetMenu( fileName = "PlayerConfig", menuName = "Configs/PlayerConfig" )]
	public class PlayerConfig : ScriptableObject
	{
		public PlayerView prefab;
		public Vector3    position;
		public Quaternion rotation;
		public float      rotationSpeed     = 3;
		public float      rotationLerpSpeed = 1;
		public float      platformDistance  = 0.05f;
		public LayerMask  platformLayerMask;
	}
}