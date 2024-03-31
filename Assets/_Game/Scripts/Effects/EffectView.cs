using System;
using UnityEngine;

namespace Scripts.Effects
{
    public class EffectView : MonoBehaviour
    {
        [SerializeField] private Transform      transformCached;
        [SerializeField] private ParticleSystem particleCached;

        public AudioSource soundSource;

        private void Reset( )
        {
            transformCached = transform;
            particleCached  = GetComponent<ParticleSystem>( );
            soundSource     = GetComponent<AudioSource>( );
        }

        public void PlayAtPosition( Vector3 position )
        {
            transformCached.position = position;
            particleCached.Play( true );

            if ( soundSource )
                soundSource.Play( );
        }
        
        public void Stop( )
        {
            particleCached.Stop( true );

            if ( soundSource )
                soundSource.Stop( );
        }
    }
}