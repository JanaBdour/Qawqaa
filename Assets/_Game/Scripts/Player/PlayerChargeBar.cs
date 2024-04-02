using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Extensions;
using UnityEngine;

namespace Scripts.Player
{
    public class PlayerChargeBar : MonoBehaviour
    {
        public GameObject gameObjectCached;
    
        [SerializeField] private SpriteRenderer fillRenderer;

        private Vector2 _size;

        private void Reset( )
        {
            gameObjectCached = gameObject;
        }

        private void Awake( )
        {
            _size = fillRenderer.size;
        }

        public void UpdateAmount( float amount )
        {
            fillRenderer.size = fillRenderer.size.WithX( Mathf.Lerp( 0, _size.x, amount ) );
        }
    }
}