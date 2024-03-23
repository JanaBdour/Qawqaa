using System;
using System.Collections.Generic;
using Scripts.Player;
using Scripts.PlayerLoop;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Scripts.Camera
{
    public class CameraService : ICameraService
    {
        public CameraView        CameraView        { get; private set; }
        public VirtualCameraView VirtualCameraView { get; private set; }

        private CameraConfig _config;

        [Inject]
        private void Construct( CameraConfig config, IPlayerLoopService playerLoopService, IPlayerService playerService )
        {
            _config = config;
            
            CameraView        = Object.Instantiate( _config.cameraViewPrefab );
            VirtualCameraView = Object.Instantiate( _config.virtualCameraPrefab );

            var player = playerService.Player.transformCached;

            VirtualCameraView.cameraCached.Follow = player;
            VirtualCameraView.cameraCached.LookAt = player;
        }
    }
}
