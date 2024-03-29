using UnityEngine;

namespace Scripts.Obstacles
{
	public class ObstacleView : MonoBehaviour 
	{
		public Transform  transformCached;
		public GameObject gameObjectCached;

		[Range( 0, 1 )] public float difficulty = 0.5f; 

 		private void Reset( )
		{
			transformCached = transform;
			gameObjectCached = gameObject;
		}
	}
}