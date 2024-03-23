using System;
using UnityEngine;

namespace Scripts.Camera
{
    public class CameraView : MonoBehaviour
    {
        public static Action<CameraView> CameraSpawned = delegate{ };

        public Transform          transformCached;
        public UnityEngine.Camera cameraObject;

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
