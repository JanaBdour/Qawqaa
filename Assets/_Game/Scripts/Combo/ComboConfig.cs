using UnityEngine;

namespace Scripts.Combo
{
	[CreateAssetMenu( fileName = "ComboConfig", menuName = "Configs/ComboConfig" )]
	public class ComboConfig : ScriptableObject
	{
		public float comboDuration        = 3f;
		public float killProgressIncrease = 0.3f;
		public float decreasingSpeed      = 0.1f;
	}
}