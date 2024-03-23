using UnityEngine;

namespace Scripts.Player
{
	[CreateAssetMenu( fileName = "PlayerConfig", menuName = "Configs/PlayerConfig" )]
	public class PlayerConfig : ScriptableObject
	{
		public PlayerView prefab;
	}
}