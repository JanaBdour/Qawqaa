using System;
using System.Collections.Generic;
using Scripts.Camera;
using Scripts.Player;
using Scripts.PlayerLoop;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using Object = UnityEngine.Object;

namespace Scripts.Shooting
{
	public class ShootingService : IShootingService
    {
        public event Action<Vector3, Vector3> OnShoot = delegate { };

        private ShootingConfig _config;
        private IPlayerService _playerService;
        private ICameraService _cameraService;

        private ShootingTrajectoryView _trajectory;
        private ShootingSimulationView _shootingSimulation;

        private PhysicsScene2D _physicsSim;
        private Scene          _simScene;
        private Vector3[]      _points;
        private Vector2        _lastForce;


        private Vector3 _startMousePosition;

        private Vector3 originPosition => _playerService.Player.transformCached.position;

        [Inject]
        private void Construct( ShootingConfig config, IPlayerService playerService, ICameraService cameraService,
            IPlayerLoopService playerLoop )
        {
            _config        = config;
            _playerService = playerService;
            _cameraService = cameraService;
            _trajectory  = Object.Instantiate( _config.trajectoryPrefab );

            _trajectory.lineCached.positionCount = _config.trajectorySteps;
            _points = new Vector3[_config.trajectorySteps];

            CreateSimulatedObject( );

            playerLoop.OnStarted    += DisableTrajectory;
            playerLoop.OnUpdateTick += HandleProjectiles;

            void CreateSimulatedObject( )
            {
                var parameters = new CreateSceneParameters( LocalPhysicsMode.Physics2D );
                _simScene        = SceneManager.CreateScene( "Simulation", parameters );
                _physicsSim      = _simScene.GetPhysicsScene2D( );
                _shootingSimulation = Object.Instantiate( _config.simulationPrefab, originPosition, Quaternion.identity );
                SceneManager.MoveGameObjectToScene( _shootingSimulation.gameObjectCached, _simScene );
            }
        }

        private void DisableTrajectory( )
        {
            _trajectory.lineCached.enabled = false;
        }

        private void HandleProjectiles( )
        {
            if ( Input.GetMouseButtonDown( 0 ) )
            {
                _startMousePosition = GetMousePosition( );
            }

            if ( Input.GetMouseButton( 0 ) )
            {
                var force = GetForce( );
                _trajectory.lineCached.enabled = force.sqrMagnitude >= _config.minForceMagnitude;
                SimulateLaunch( force );
            }

            if ( Input.GetMouseButtonUp( 0 ) )
            {
                _trajectory.lineCached.enabled = false;
                OnShoot.Invoke( originPosition, GetForce( ) );
            }
        }

        private Vector3 GetMousePosition( )
        {
            return new Vector3( Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height );
        }
        
        private Vector3 GetForce( )
        {
            var direction = -( GetMousePosition( ) - _startMousePosition );
            var force     = new Vector3( direction.x * _config.throwForce.x, direction.y * _config.throwForce.y );

            force.x = Mathf.Clamp( force.x, 0, _config.maxForce.x );
            force.y = Mathf.Clamp( force.y, -_config.maxForce.y * 0.5f, _config.maxForce.y );

            return force;
        }
        
        private void SimulateLaunch( Vector2 force )
        {
            _shootingSimulation.transformCached.position = originPosition;
            _shootingSimulation.transformCached.rotation = Quaternion.identity;
            _shootingSimulation.rigidbodyCached.velocity = Vector2.zero;

            if ( _lastForce != force )
            {
                _shootingSimulation.rigidbodyCached.AddForce( force, ForceMode2D.Impulse );
                for ( var index = 0; index < _config.trajectorySteps; index++ )
                {
                    _physicsSim.Simulate( Time.fixedDeltaTime );
                    _points[index] = _shootingSimulation.transformCached.position;
                    _trajectory.lineCached.SetPosition( index, _points[index] );
                }
            }

            _lastForce = force;
        }

        private bool CheckForCollision( Vector2 position )
        {
            var hits = Physics2D.OverlapCircleAll( position, _config.collisionCheckRadius ); 
            return hits.Length > 0;
        }
    }
}