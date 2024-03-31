using System;
using System.Collections.Generic;
using DG.Tweening;
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

        private CameraConfig   _config;
        private IPlayerService _playerService;
        private float          _zoomFactor;

        [Inject]
        private void Construct( CameraConfig config, IPlayerLoopService playerLoopService,
            IPlayerService                   playerService )
        {
            _config        = config;
            _playerService = playerService;

            CameraView        = Object.Instantiate( _config.cameraViewPrefab );
            VirtualCameraView = Object.Instantiate( _config.virtualCameraPrefab );

            var player = playerService.Player.transformCached;

            VirtualCameraView.cameraCached.Follow = player;
            VirtualCameraView.cameraCached.LookAt = player;

            playerLoopService.OnUpdateTick += HandleZoom;
        }

        private void HandleZoom( )
        {
            if ( DOTween.IsTweening( VirtualCameraView.cameraCached.m_Lens.FieldOfView ) ) return;
            
            var currentZoomFactor = _playerService.Player.rigidbodyCached.velocity.magnitude / _config.maxVelocityMagnitude;
            _zoomFactor = Mathf.Lerp( _zoomFactor, currentZoomFactor, Time.deltaTime * _config.zoomLerpSpeed );
            VirtualCameraView.cameraCached.m_Lens.FieldOfView =
                Mathf.Lerp( _config.minOrthoSize, _config.maxOrthoSize, _zoomFactor );
        }
    }
}