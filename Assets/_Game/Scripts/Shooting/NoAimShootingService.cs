using System;
using Scripts.Camera;
using Scripts.Player;
using Scripts.PlayerLoop;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using Object = UnityEngine.Object;

namespace Scripts.Shooting
{
    public class NoAimShootingService : IShootingService
    {
         public event Action<Vector3> OnShoot = delegate { };
        public event  Action<Vector3> OnAim   = delegate { };

        private ShootingConfig     _config;
        private IPlayerService     _playerService;
        private ICameraService     _cameraService;
        private IPlayerLoopService _playerLoopService;

        private int _upCounter;

        private Vector3 originPosition => _playerService.Player.shootPositionTransform.position;

        [Inject]
        private void Construct( ShootingConfig config, IPlayerService playerService, ICameraService cameraService,
            IPlayerLoopService playerLoop )
        {
            _config            = config;
            _playerService     = playerService;
            _cameraService     = cameraService;
            _playerLoopService = playerLoop;

            playerService.OnHitGround += ResetCounter;
            playerLoop.OnStarted      += DisableAndResetTrajectory;
            playerLoop.OnUpdateTick   += HandleProjectiles;
        }

        private void ResetCounter( )
        {
            if ( _upCounter == 0 )
                _upCounter = _config.maxUpJumps;
        }
        
        private void DisableAndResetTrajectory( )
        {
            _upCounter = _config.maxUpJumps;
        }

        private void HandleProjectiles( )
        {
            if ( !_playerLoopService.IsPlaying || _playerLoopService.GameplayTime < 0.1f ) return;
            
            if ( Input.GetMouseButtonDown( 0 ) )
            {
                OnShoot.Invoke( GetForce( ) );
                _upCounter--;
            }
        }

        private Vector3 GetForce( )
        {
            var force = new Vector3( _config.throwForce.x, _upCounter > 0 ? _config.throwForce.y : 0 );

            return force;
        }
    }
}