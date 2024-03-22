using System;
using System.Collections.Generic;
using Scripts.Ticking;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Scripts.Camera
{
    public class CameraService : ICameraService
    {
        public CameraView        CameraView   { get; private set; }
        public CameraStateConfig CurrentState { get; private set; }

        public event Action OnCameraPositionSettled = delegate { };

        private CameraConfig _config;
        
        private Dictionary<string, CameraStateConfig> _states;

        private Vector3 _center;
        private float   _zoomMultiplier = 1;
        private float   _timer;

        [Inject]
        private void Construct( CameraConfig config, ITickingService tickingService )
        {
            _config = config;
            
            CameraView = Object.Instantiate( _config.cameraViewPrefab );

            _states = new Dictionary<string, CameraStateConfig>( );
            foreach ( var state in _config.states )
                _states.Add( state.stateId, state );
            CurrentState = _states["Default"];

            tickingService.OnStarted    += Reset;
            tickingService.OnUpdateTick += UpdateCamera;
        }
        
        private void Reset()
        {
            CameraView.positionTransform.localPosition = CurrentState.movementOffset + _center;
            CameraView.rotationTransform.rotation      = Quaternion.Euler( CurrentState.rotation );
            CameraView.distanceTransform.localPosition = -CurrentState.distance * Vector3.forward;
            CameraView.cameraObject.fieldOfView        = CurrentState.fieldOfView;
            CameraView.cameraObject.nearClipPlane      = CurrentState.clipping.x;
            CameraView.cameraObject.farClipPlane       = CurrentState.clipping.y;
        }

        private void UpdateCamera( )
        {
            if ( CameraView == null || CurrentState == null )
                return;

            CameraView.transformCached.position        = Vector3.Lerp   ( CameraView.transformCached.position           , _center                                   , Time.deltaTime * _config.moveSpeed );
            CameraView.positionTransform.localPosition = Vector3.Lerp   ( CameraView.positionTransform.localPosition    , CurrentState.movementOffset               , Time.deltaTime * CurrentState.movementChangeSpeed );
            CameraView.rotationTransform.rotation      = Quaternion.Lerp( CameraView.rotationTransform.rotation         , Quaternion.Euler( CurrentState.rotation ) , Time.deltaTime * CurrentState.rotationChangeSpeed );
            CameraView.distanceTransform.localPosition = Mathf.Lerp     ( CameraView.distanceTransform.localPosition.z  , -CurrentState.distance * _zoomMultiplier  , Time.deltaTime * CurrentState.distanceChangeSpeed ) * Vector3.forward;
            CameraView.cameraObject.fieldOfView        = Mathf.Lerp     ( CameraView.cameraObject.fieldOfView           , CurrentState.fieldOfView                  , Time.deltaTime * CurrentState.clippingChangeSpeed );
            CameraView.cameraObject.nearClipPlane      = Mathf.Lerp     ( CameraView.cameraObject.nearClipPlane         , CurrentState.clipping.x                   , Time.deltaTime * CurrentState.clippingChangeSpeed );
            CameraView.cameraObject.farClipPlane       = Mathf.Lerp     ( CameraView.cameraObject.farClipPlane          , CurrentState.clipping.y                   , Time.deltaTime * CurrentState.clippingChangeSpeed );

            HandleMove( );
          //  HandleRotate( );
            HandleZoom( );
        }
        
        public void ChangeState( string id )
        {
            if ( _states.ContainsKey( id ) )
                CurrentState = _states[id];
        }
        
        public void ChangeCenter( Vector3 center )
        {
            _center = center;
        }
        
        private void HandleMove( )
        {
            var mouseX = Input.GetAxis( "Mouse X" );
            var mouseY = Input.GetAxis( "Mouse Y" );

            var movement = new Vector3( -mouseX, 0, -mouseY );
            if ( Input.GetMouseButton( 0 ) )
            {
                var transform = CameraView.rotationTransform;
                _center   += transform.forward * movement.z + transform.right * movement.x;
                _center.y =  0;
            }

            var isMoving = mouseX > 0 && mouseY > 0;
            if ( isMoving )
                _timer = 0;
            else
                _timer += Time.deltaTime;

            if ( _timer >= _config.settlingDuration )
                OnCameraPositionSettled.Invoke( );
        }

        private void HandleZoom( )
        {
            var scroll = Input.GetAxis( "Mouse ScrollWheel" );
            _zoomMultiplier += scroll * _config.zoomSpeed * Time.deltaTime;
        }
    }
}
