using System;
using Scripts.Extensions;
using UnityEngine;

namespace Scripts.Platforms
{
    public class PlatformView : MonoBehaviour
    {
        public Transform     transformCached;
        public GameObject    gameObjectCached;
        public BoxCollider2D colliderCached;
        public Renderer      rendererCached;

        private void Reset( )
        {
            transformCached  = transform;
            gameObjectCached = gameObject;
            colliderCached   = GetComponentInChildren<BoxCollider2D>( );
            rendererCached   = GetComponentInChildren<Renderer>( );
        }
    }
}