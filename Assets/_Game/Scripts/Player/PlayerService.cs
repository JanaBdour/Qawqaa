using System;
using Scripts.Extensions;
using Scripts.Platforms;
using Scripts.PlayerLoop;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Scripts.Player
{
	public class PlayerService : IPlayerService
	{
		public PlayerView   Player { get; private set; }
		public event Action OnMove = delegate { };

		public void OnHitObstacle( )
		{
			_playerLoopService.EndLoop( );
		}

		private PlayerConfig       _config;
		private IPlayerLoopService _playerLoopService;
		private IPlatformsService  _platformsService;
		private int                _jumpCounter;

		[Inject]
		private void Construct( PlayerConfig config, IPlayerLoopService playerLoopService, IPlatformsService platformsService )
		{
			_config = config;
			Player  = Object.Instantiate( _config.prefab );
			
			Player.Initialize( this, _config );

			_playerLoopService = playerLoopService;
			_platformsService  = platformsService;

			playerLoopService.OnStarted    += Reset;
			playerLoopService.OnUpdateTick += Update;
		}

		private void Reset( )
		{
			Player.ResetValues( );
			
			Player.transformCached.position    = _config.position;
			Player.transformCached.rotation    = _config.rotation;
			Player.rigidbodyCached.velocity    = Vector2.zero;
			Player.rigidbodyCached.isKinematic = false;

			_jumpCounter = _config.maxJumpCount;
		}

		private void Update( )
		{
			if ( !_playerLoopService.IsPlaying || _playerLoopService.GameplayTime < 0.1f ) return;

			HandleMoving( );
			HandleJumpResetting( );
			HandleDying( );
		}

		private void HandleMoving( )
		{
			if ( !Input.GetMouseButtonDown( 0 ) ) return;
			
			var force = new Vector3( _config.throwForce.x, _jumpCounter > 0 ? _config.throwForce.y : 0 );

			Player.rigidbodyCached.AddForce( force );
			_jumpCounter--;

			OnMove.Invoke( );
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
			_playerLoopService.EndLoop( );
		}
	}
}