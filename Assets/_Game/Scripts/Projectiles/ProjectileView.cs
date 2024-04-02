using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts.Projectiles
{
    public class ProjectileView : MonoBehaviour
    {
        public Transform   transformCached;
        public GameObject  gameObjectCached;
        public Rigidbody2D rigidbodyCached;
        public AudioSource shootAudioSource;

        [SerializeField] private Transform localMeshTransform;
        
        private ProjectilesConfig   _config;
        private IProjectilesService _projectilesService;
        private float               _lifetimeTimer;
        private Vector3             _rotationDirection;

        private void Reset( )
        {
            transformCached  = transform;
            gameObjectCached = gameObject;
            rigidbodyCached  = GetComponent<Rigidbody2D>( );
        }

        public void Initialize( ProjectilesConfig config, IProjectilesService projectilesService )
        {
            _config             = config;
            _projectilesService = projectilesService;

            ResetProperties( );
        }

        public void ResetProperties( )
        {
            _lifetimeTimer     = _config.maxLifetime;
            _rotationDirection = new Vector3( Random.Range( -1, 1 ), Random.Range( -1, 1 ), Random.Range( -1, 1 ) );
            shootAudioSource.Play( );
        }

        private void Update( )
        {
            _lifetimeTimer -= Time.deltaTime;

            var scale = _config.normalScale + Mathf.Lerp( 0,
                _config.pulseAddedScale * ( _lifetimeTimer / _config.maxLifetime ),
                Mathf.PingPong( Time.time * _config.pulseSpeed, 1 ) );
            transformCached.localScale = scale * Vector3.one;
            
            localMeshTransform.Rotate( _rotationDirection * Time.deltaTime * _config.localRotationSpeed, Space.Self );

            if ( _lifetimeTimer <= 0 )
                _projectilesService.Explode( this );
        }

        private void OnCollisionEnter2D( Collision2D col )
        {
            _projectilesService.Explode( this );
        }
    }
}