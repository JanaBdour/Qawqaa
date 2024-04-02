using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts.Collectible
{
    public class CollectibleView : MonoBehaviour
    {
        public Transform  transformCached;
        public GameObject gameObjectCached;

        private CollectibleConfig _config;
        private Vector3           _startPosition;
        private float             _animationDistance;
        private float             _animationSpeed;
        
        private void Reset( )
        {
            transformCached  = transform;
            gameObjectCached = gameObject;
        }

        public void Initialize( CollectibleConfig config )
        {
            _config = config;
            ResetProperties( );

        }

        public void ResetProperties( )
        {
            _startPosition     = transformCached.position;
            _animationDistance = Random.Range( _config.minAnimationDistance, _config.maxAnimationDistance );
            _animationSpeed    = Random.Range( _config.minAnimationSpeed, _config.maxAnimationSpeed );
        }

        private void Update( )
        {
            var position = _startPosition;
            position.y = Mathf.Lerp( position.y, position.y - _animationDistance,
                Mathf.PingPong( Time.time * _animationSpeed, 1 ) );

            transformCached.position = position;
        }
    }
}