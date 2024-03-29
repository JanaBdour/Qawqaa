using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts.Enemy
{
	public class EnemyView : MonoBehaviour
	{
		public Transform      meshTransform;
		public ParticleSystem deathEffect;

		[SerializeField] private Renderer    rendererCached;
		[SerializeField] private Rigidbody2D rigidbodyCached;
		[SerializeField] private Collider2D  colliderCached;

		private EnemyConfig _config;
		private float       _timer;
		private bool        _isDead;

		private void Reset( )
		{
			deathEffect     = GetComponentInChildren<ParticleSystem>( );
			rendererCached  = GetComponent<Renderer>( );
			rigidbodyCached = GetComponentInChildren<Rigidbody2D>( );
			colliderCached  = GetComponentInChildren<Collider2D>( );
		}

        public void Initialize( EnemyConfig config )
        {
	        _config = config;
	        RandomizeTimer( );
        }

        public void Die( )
        {
	        if ( _isDead ) return;

	        _isDead = true;
	        
	        deathEffect.Play( true );
	        rendererCached.enabled = false;
	        colliderCached.enabled = false;
        }

        private void Update( )
        {
	        _timer -= Time.deltaTime;
	        if ( _timer > 0 ) return;

	        rigidbodyCached.AddForce( Vector2.up * Random.Range( _config.minJumpForce, _config.maxJumpForce ), ForceMode2D.Impulse );
	        RandomizeTimer( );
        }

        private void RandomizeTimer( )
        {
	        _timer = Random.Range( _config.waitMinDuration, _config.waitMaxDuration );
        }
	}
}