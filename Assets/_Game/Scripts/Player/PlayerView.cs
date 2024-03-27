using UnityEngine;

namespace Scripts.Player
{
    public class PlayerView : MonoBehaviour
    {
        public Transform   shootPositionTransform;
        public Transform   transformCached;
        public Transform   shellTransform;
        public GameObject  gameObjectCached;
        public Rigidbody2D rigidbodyCached;

        private void Reset( )
        {
            transformCached  = transform;
            gameObjectCached = gameObject;
            rigidbodyCached  = GetComponent<Rigidbody2D>( );
        }
    }
}