using UnityEngine;

namespace Scripts.Characters
{
    public class CharacterView : MonoBehaviour
    {
        public CharacterController controller;
        public Transform           transformCached;
        public GameObject          gameObjectCached;
        public Renderer            rendererCached;

        private void Reset( )
        {
            controller       = GetComponent<CharacterController>( );
            transformCached  = transform;
            gameObjectCached = gameObject;
            rendererCached   = GetComponentInChildren<Renderer>( );
        }
    }
}
