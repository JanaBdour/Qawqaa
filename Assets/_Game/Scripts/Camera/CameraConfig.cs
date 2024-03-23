using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Camera
{
    [CreateAssetMenu( fileName = "CameraConfig", menuName = "Configs/CameraConfig" )]
    public class CameraConfig : ScriptableObject
    {
        public CameraView        cameraViewPrefab;
        public VirtualCameraView virtualCameraPrefab;
            
        public float moveSpeed        = 10f;
        public float zoomSpeed        = 10f;
        public float settlingDuration = 2; 
    }
}
