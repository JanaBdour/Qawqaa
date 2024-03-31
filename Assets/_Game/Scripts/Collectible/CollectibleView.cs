using UnityEngine;

namespace Scripts.Collectible
{
	public class CollectibleView : MonoBehaviour 
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