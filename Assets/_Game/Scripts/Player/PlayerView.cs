using System;
using System.Collections;
using DG.Tweening;
using MoreMountains.Feedbacks;
using Scripts.Extensions;
using UnityEngine;

namespace Scripts.Player
{
    public class PlayerView : MonoBehaviour
    {
        public Transform     shootPositionTransform;
        public Transform     transformCached;
        public Transform     shellTransform;
        public Rigidbody2D   rigidbodyCached;
        public AudioSource[] audioSources;

        [SerializeField] private MeshRenderer   rendererCached;
        [SerializeField] private ParticleSystem comboEffect;
        [SerializeField] private TrailRenderer  trailRendererCached;
        [SerializeField] private MMF_Player     deathFeedbacks;
        [SerializeField] private MMF_Player     moveFeedbacks;
        [SerializeField] private AudioSource    rotatingAudioSource;

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
            rigidbodyCached.simulated   = true;

            _isDead = false;

            comboEffect.Stop( true );
            
            rendererCached.sharedMaterial.SetFloat( "_FresnelStrength", 0 );
            StartCoroutine( DelayEnablingTrailRenderer( ) );
        }

        public void Move( Vector3 force )
        {
            rigidbodyCached.AddForce( force );
            moveFeedbacks.PlayFeedbacks( );
        }

        public void Die( )
        {
            _isDead = true;

            rendererCached.enabled    = false;
            rigidbodyCached.simulated = false;

            comboEffect.Stop( true );
            deathFeedbacks.PlayFeedbacks( );
            trailRendererCached.Reset( this, true );
        }

        public void StartCombo( bool start, float fresnelDuration )
        {
            rendererCached.sharedMaterial.DOFloat( start ? 1 : 0, "_FresnelStrength", fresnelDuration );
            if ( start )
            {
                comboEffect.Play( true );
                rotatingAudioSource.Play( );
            }
            else
            {
                comboEffect.Stop( true );
                rotatingAudioSource.Pause( );
            }
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
                _playerService.OnHitObstacle( col.collider );
            }
        }
    }
}