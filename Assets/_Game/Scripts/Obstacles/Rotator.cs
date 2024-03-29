using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Obstacles
{
    public class Rotator : MonoBehaviour
    {
        [SerializeField] private float   speed     = 20;
        [SerializeField] private Vector3 direction = new Vector3( 0, 0, 1 );

        private Transform _transform;

        private void Awake( )
        {
            _transform = transform;
        }

        private void Update( )
        {
            _transform.Rotate( speed * direction * Time.deltaTime );
        }
    }
}