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
        private bool           _isDead;
        
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
            rendererCached.enabled      = true;
            trailRendererCached.enabled = true;

            _isDead = false;
        }

        private void OnCollisionEnter2D( Collision2D col )
        {
            if ( col.collider.CompareTag( "Obstacle" ) && !_isDead )
            {
                _isDead = true;
                deathEffect.Play( true );
                rendererCached.enabled = false;
                _playerService.OnHitObstacle( );
                trailRendererCached.Reset( this, true );
            }
        }
    }
}