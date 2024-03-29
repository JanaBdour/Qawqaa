using UnityEngine;

namespace Scripts.Enemy
{
	[CreateAssetMenu( fileName = "EnemyConfig", menuName = "Configs/EnemyConfig" )]
	public class EnemyConfig : ScriptableObject
	{
		public float waitMinDuration = 0.9f;
		public float waitMaxDuration = 1.3f;
		public float minJumpForce    = 1;
		public float maxJumpForce    = 3;
		public float killDistance    = 0.5f;
	}
}