using System;
using Scripts.Combo;
using Scripts.Extensions;
using Scripts.Feedback;
using Scripts.Platforms;
using Scripts.PlayerLoop;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Scripts.Player
{
	public class PlayerService : IPlayerService
	{
		public PlayerView   Player { get; private set; }
		public event Action OnMove     = delegate { };
		public event Action OnLongMove = delegate { };

		public void OnHitObstacle( Collider2D obstacle )
		{
			if ( !_comboService.IsCombo )
			{
				_playerLoopService.EndLoop( );
				Player.Die( );
			}
			else if(obstacle.TryGetComponent<Rigidbody2D>( out Rigidbody2D obstacleRigidbody ))
			{
				obstacleRigidbody.AddForce( Random.insideUnitCircle * _config.comboObstacleForceRadius *
				                            _config.comboObstacleForce );
			}
		}

		private PlayerConfig       _config;
		private IPlayerLoopService _playerLoopService;
		private IPlatformsService  _platformsService;
		private IComboService      _comboService;
		private Quaternion         _shellStartRotation;
		private int                _jumpCounter;
		private float              _startClickTime;

		[Inject]
		private void Construct( PlayerConfig config, IPlayerLoopService playerLoopService, IPlatformsService platformsService, IComboService comboService, IFeedbackService feedbackService )
		{
			_config = config;
			Player  = Object.Instantiate( _config.prefab );
			
			Player.Initialize( this, _config );
			foreach ( var source in Player.audioSources )
			{
				feedbackService.RegisterAudioSource( source );
			}

			_playerLoopService  = playerLoopService;
			_platformsService   = platformsService;
			_comboService       = comboService;
			_shellStartRotation = Player.shellTransform.localRotation;

			comboService.OnStartCombo      += ( ) => Player.StartCombo( true, _config.fresnelFadeDuration );
			comboService.OnEndCombo        += ( ) => Player.StartCombo( false, _config.fresnelFadeDuration );
			playerLoopService.OnStarted    += Reset;
			playerLoopService.OnUpdateTick += Update;
		}

		private void Reset( )
		{
			Player.ResetValues( );
			
			Player.transformCached.position     = _config.position;
			Player.transformCached.rotation     = _config.rotation;
			Player.rigidbodyCached.velocity     = Vector2.zero;
			Player.rigidbodyCached.isKinematic  = false;
			Player.shellTransform.localRotation = _shellStartRotation;

			_jumpCounter = _config.maxJumpCount;
		}

		private void Update( )
		{
			if ( !_playerLoopService.IsPlaying || _playerLoopService.GameplayTime < 0.1f ) return;

			HandleComboRotating( );
			HandleMoving( );
			HandleJumpResetting( );
			HandleDying( );
		}

		private void HandleComboRotating( )
		{
			if ( _comboService.IsCombo )
			{
				var rotationDirection = Vector3.up * Time.deltaTime * _config.comboRotatingSpeed;
				Player.shellTransform.Rotate( Vector3.Lerp( rotationDirection, Vector3.zero, _comboService.ComboProgress ), Space.Self );
			}
			else
			{
				Player.shellTransform.localRotation = Quaternion.Lerp
				(
					Player.shellTransform.localRotation,
					_shellStartRotation,
					Time.deltaTime * _config.comboResettingSpeed
				);
			}
		}
		
		private void HandleMoving( )
		{
			if ( Input.GetMouseButtonDown( 0 ) )
			{
				HandleClicking( );
			}
			else if ( Input.GetMouseButton( 0 ) )
			{
				HandleHolding( );
			}
			else if ( Input.GetMouseButtonUp( 0 ) )
			{
				HandleReleasing( );
			}
			
			void HandleClicking( )
			{
				_startClickTime = Time.time;
				Player.barCached.gameObjectCached.SetActive( false );
			}

			void HandleHolding( )
			{
				var duration = Time.time - _startClickTime;
				var progress =  duration / _config.holdDuration;

				if ( duration > _config.tapDuration )
				{
					Player.barCached.gameObjectCached.SetActive( true );
					Player.barCached.UpdateAmount( progress );
				}
			}

			void HandleReleasing( )
			{
				Player.barCached.gameObjectCached.SetActive( false );

				var duration = Time.time - _startClickTime;

				if ( duration <= _config.tapDuration )
				{
					var force = _jumpCounter <= 0 ? _config.throwForce.ZeroY( ) : _config.throwForce;

					Player.Move( force );
					_jumpCounter--;

					OnMove.Invoke( );
				}
				else if ( duration >= _config.holdDuration )
				{
					Player.Move( Vector2.Scale( _config.throwForce, _config.longMoveMultiplier ) );

					OnLongMove.Invoke( );
				}
			}
		}

		private void HandleJumpResetting( )
		{
			if ( _jumpCounter > 0 || !Physics2D.Raycast( Player.transformCached.position.AddY( 0.05f ), -Vector2.up,
				    _config.platformDistance, _config.platformLayerMask ) ) return;

			_jumpCounter = _config.maxJumpCount;
		}

		private void HandleDying( )
		{
			if ( Player.rigidbodyCached.isKinematic || Player.rigidbodyCached.velocity.y >= 0 ) return;

			var closestPlatform = _platformsService.GetClosestPlatformOnX( Player.transformCached.position.x );
			if ( closestPlatform.transformCached.position.y - closestPlatform.colliderCached.bounds.extents.y + closestPlatform.colliderCached.offset.y <=
			     Player.transformCached.position.y ) return;
			
			Player.rigidbodyCached.isKinematic = true;
			Player.Die( );
			_playerLoopService.EndLoop( );
		}
	}
}