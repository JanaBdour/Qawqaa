using System;
using Scripts.Extensions;
using UnityEngine;

namespace Scripts.Player
{
    public class PlayerView : MonoBehaviour
    {
        public Transform   shootPositionTransform;
        public Transform   transformCached;
        public Transform   shellTransform;
        public GameObject  gameObjectCached;
        public Rigidbody2D rigidbodyCached;

        [SerializeField] private ParticleSystem deathEffect;
        [SerializeField] private MeshRenderer   rendererCached;
        [SerializeField] private TrailRenderer  trailRendererCached;

        private IPlayerService _playerService;
        
        private void Reset( )
        {
            transformCached     = transform;
            gameObjectCached    = gameObject;
            rigidbodyCached     = GetComponent<Rigidbody2D>( );
            deathEffect         = GetComponentInChildren<ParticleSystem>( );
            rendererCached      = GetComponent<MeshRenderer>( );
            trailRendererCached = GetComponent<TrailRenderer>( );
        }

        public void Initialize( IPlayerService playerService )
        {
            _playerService = playerService;
        }

        public void ResetValues( )
        {
            deathEffect.Stop( true );
            rendererCached.enabled = true;
            trailRendererCached.Reset( this );
        }

        private void OnCollisionEnter2D( Collision2D col )
        {
            if ( !col.collider.CompareTag( "Obstacle" ) ) return;
            
            deathEffect.Play( true );
            rendererCached.enabled = false;
            _playerService.OnHitObstacle( );
        }
    }
}