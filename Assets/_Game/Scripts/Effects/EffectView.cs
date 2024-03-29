using System;
using UnityEngine;

namespace Scripts.Effects
{
    public class EffectView : MonoBehaviour
    {
        [SerializeField] private Transform      transformCached;
        [SerializeField] private ParticleSystem particleCached;

        private void Reset( )
        {
            transformCached = transform;
            particleCached  = GetComponent<ParticleSystem>( );
        }

        public void PlayAtPosition( Vector3 position )
        {
            transformCached.position = position;
            particleCached.Play( true );
        }
        
        public void Stop( )
        {
            particleCached.Stop( true );
        }
    }
}