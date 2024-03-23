using UnityEngine;

namespace Scripts.Shooting
{
	public class ShootingSimulationView : MonoBehaviour 
	{
		public Transform   transformCached;
		public GameObject  gameObjectCached;
		public Rigidbody2D rigidbodyCached;

 		private void Reset( )
		{
			transformCached  = transform;
			gameObjectCached = gameObject;
			rigidbodyCached  = GetComponent<Rigidbody2D>( );
		}
	}
}