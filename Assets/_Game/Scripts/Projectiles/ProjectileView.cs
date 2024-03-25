using System;
using UnityEngine;

namespace Scripts.Projectiles
{
    public class ProjectileView : MonoBehaviour
    {
        public Transform   transformCached;
        public GameObject  gameObjectCached;
        public Rigidbody2D rigidbodyCached;

        private ProjectilesConfig   _config;
        private IProjectilesService _projectilesService;
        private float               _lifetimeTimer;

        private void Reset( )
        {
            transformCached  = transform;
            gameObjectCached = gameObject;
            rigidbodyCached  = GetComponent<Rigidbody2D>( );
        }

        public void Initialize( ProjectilesConfig config, IProjectilesService projectilesService )
        {
            _config             = config;
            _lifetimeTimer      = _config.maxLifetime;
            _projectilesService = projectilesService;
        }

        private void Update( )
        {
            _lifetimeTimer -= Time.deltaTime;

            var scale = _config.normalScale + Mathf.Lerp( 0,
                _config.pulseAddedScale * ( _lifetimeTimer / _config.maxLifetime ),
                Mathf.PingPong( Time.time * _config.pulseSpeed, 1 ) );
            transformCached.localScale = scale * Vector3.one;

            if ( _lifetimeTimer <= 0 )
                _projectilesService.Explode( this );
        }

        private void OnCollisionEnter2D( Collision2D col )
        {
            _projectilesService.Explode( this );
        }
    }
}