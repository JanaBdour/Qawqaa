using UnityEngine;

namespace Scripts.Player
{
	public class PlayerView : MonoBehaviour 
	{
		public Transform transformCached;
 		public GameObject gameObjectCached;

 		private void Reset( )
		{
			transformCached = transform;
			gameObjectCached = gameObject;
		}
	}
}