using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Extensions;
using UnityEngine;

namespace Scripts.Animations
{
    public class HoverAnimator : MonoBehaviour
    {
        [SerializeField] private float speed    = 1;
        [SerializeField] private float distance = 0.01f;

        private Transform _transform;
        private Vector3   _position;

        private void Awake( )
        {
            _transform = transform;
            _position  = _transform.position;
        }

        private void Update( )
        {
            _transform.position = _position.AddY( Mathf.Lerp( -distance, distance, Mathf.PingPong( Time.time * speed, 1 ) ) );
        }
    }
}