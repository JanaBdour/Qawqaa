using System;
using UnityEngine;

namespace Scripts.FallingSteps
{
    public class FallingStepView : MonoBehaviour
    {
        public Transform   transformCached;
        public GameObject  gameObjectCached;
        public Rigidbody2D rigidbodyCached;

        private void Reset( )
        {
            transformCached  = transform;
            gameObjectCached = gameObject;
            rigidbodyCached  = GetComponent<Rigidbody2D>( );
        }

        private void Awake( )
        {
            rigidbodyCached.isKinematic = true;
        }

        private void OnCollisionEnter2D( Collision2D collision )
        {
            rigidbodyCached.isKinematic = false;
        }
    }
}