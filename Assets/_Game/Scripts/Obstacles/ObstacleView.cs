using UnityEngine;

namespace Scripts.Obstacles
{
	public class ObstacleView : MonoBehaviour 
	{
		public Transform  transformCached;
		public GameObject gameObjectCached;
		
		private void Reset( )
		{
			transformCached = transform;
			gameObjectCached = gameObject;
		}
	}
}