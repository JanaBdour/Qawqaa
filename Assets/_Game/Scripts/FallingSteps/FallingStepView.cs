using System;
using UnityEngine;
using UnityEngine.Pool;

namespace Scripts.FallingSteps
{
    public class FallingStepView : MonoBehaviour
    {
        public Transform   transformCached;
        public GameObject  gameObjectCached;

        [SerializeField] private Rigidbody2D rigidbodyCached;

        private ObjectPool<FallingStepView> _pool;
        private FallingStepsConfig          _config;
        private float                       _releaseTimer;

        private void Reset( )
        {
            transformCached  = transform;
            gameObjectCached = gameObject;
            rigidbodyCached  = GetComponent<Rigidbody2D>( );
        }

        public void Initialize( ObjectPool<FallingStepView> pool, FallingStepsConfig config )
        {
            _pool   = pool;
            _config = config;
            ResetProperties( );
        }

        public void ResetProperties( )
        {
            rigidbodyCached.isKinematic = true;
        }

        private void OnCollisionEnter2D( Collision2D collision )
        {
            rigidbodyCached.isKinematic = false;
            _releaseTimer               = _config.releaseWait;
        }

        private void Update( )
        {
            if ( rigidbodyCached.isKinematic ) return;

            _releaseTimer -= Time.deltaTime;
            if ( _releaseTimer <= 0 )
                _pool.Release( this );
        }
    }
}