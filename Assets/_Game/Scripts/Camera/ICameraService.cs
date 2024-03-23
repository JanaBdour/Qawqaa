using System;
using UnityEngine;

namespace Scripts.Camera
{
    public interface ICameraService
    {
        CameraView        CameraView        { get; }
        VirtualCameraView VirtualCameraView { get; }
    }
}
