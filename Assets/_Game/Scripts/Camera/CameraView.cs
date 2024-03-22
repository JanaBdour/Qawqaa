using System;
using UnityEngine;

namespace Scripts.Camera
{
    public class CameraView : MonoBehaviour
    {
        public static Action<CameraView> CameraSpawned = delegate{ };

        public Transform          transformCached;
        public UnityEngine.Camera cameraObject;

        public Transform positionTransform;
        public Transform rotationTransform;
        public Transform distanceTransform;
        
        private void Reset( )
        {
            transformCached = transform;
            cameraObject    = GetComponent<UnityEngine.Camera>( );
        }

        private void Start()
        {
            CameraSpawned.Invoke( this );
        }
    }
}
