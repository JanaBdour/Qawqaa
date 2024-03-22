using System;
using UnityEngine;

namespace Scripts.Camera
{
    public interface ICameraService
    {
         CameraView        CameraView   { get; }
         CameraStateConfig CurrentState { get; }

         void ChangeState( string   id );
         void ChangeCenter( Vector3 center );

         event Action OnCameraPositionSettled;
    }
}
