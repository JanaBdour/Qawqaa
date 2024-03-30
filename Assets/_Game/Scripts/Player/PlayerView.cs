using System;
using System.Collections;
using MoreMountains.Feedbacks;
using Scripts.Extensions;
using UnityEngine;

namespace Scripts.Player
{
    public class PlayerView : MonoBehaviour
    {
        public Transform   shootPositionTransform;
        public Transform   transformCached;
        public Rigidbody2D rigidbodyCached;

        [SerializeField] private MeshRenderer   rendererCached;
        [SerializeField] private TrailRenderer  trailRendererCached;
        [SerializeField] private MMF_Player     deathFeedbacks;

        private IPlayerService _playerService;
        private PlayerConfig   _config;
        private bool           _isDead;
        
        private void Reset( )
        {
            transformCached     = transform;
            rigidbodyCached     = GetComponent<Rigidbody2D>( );
            rendererCached      = GetComponent<MeshRenderer>( );
            trailRendererCached = GetComponent<TrailRenderer>( );
        }

        public void Initialize( IPlayerService playerService, PlayerConfig playerConfig )
        {
            _playerService = playerService;
            _config        = playerConfig;
        }

        public void ResetValues( )
        {
            rendererCached.enabled      = true;
            trailRendererCached.enabled = false;

            _isDead = false;

            StartCoroutine( DelayEnablingTrailRenderer( ) );
        }

        IEnumerator DelayEnablingTrailRenderer( )
        {
            yield return new WaitForSeconds( _config.trailEnableDelay );
            trailRendererCached.enabled = true;
        }

        private void OnCollisionEnter2D( Collision2D col )
        {
            if ( col.collider.CompareTag( "Obstacle" ) && !_isDead )
            {
                _isDead = true;

                rendererCached.enabled = false;

                deathFeedbacks.PlayFeedbacks( );
                _playerService.OnHitObstacle( );
                trailRendererCached.Reset( this, true );
            }
        }
    }
}