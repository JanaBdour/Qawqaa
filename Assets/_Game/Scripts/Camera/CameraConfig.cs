using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Camera
{
    [CreateAssetMenu( fileName = "CameraConfig", menuName = "Configs/CameraConfig" )]
    public class CameraConfig : ScriptableObject
    {
        public CameraView        cameraViewPrefab;
        public VirtualCameraView virtualCameraPrefab;

        public float zoomLerpSpeed        = 5;
        public float maxVelocityMagnitude = 10;
        public float minOrthoSize         = 50;
        public float maxOrthoSize         = 80;
    }
}
