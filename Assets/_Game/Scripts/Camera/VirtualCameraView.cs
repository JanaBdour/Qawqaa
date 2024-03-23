using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace Scripts.Camera
{
    public class VirtualCameraView : MonoBehaviour
    {
        public CinemachineVirtualCamera cameraCached;

        private void Reset( )
        {
            cameraCached = GetComponent<CinemachineVirtualCamera>( );
        }
    }
}