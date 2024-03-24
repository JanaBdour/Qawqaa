using Scripts.Platforms;
using Scripts.PlayerLoop;
using Scripts.Shooting;
using UnityEngine;
using Zenject;

namespace Scripts.Player
{
	public class PlayerService : IPlayerService
	{
		public PlayerView Player { get; private set; }
	
		private PlayerConfig       _config;
		private IPlayerLoopService _playerLoopService;
		private IPlatformsService  _platformsService;

		[Inject]
		private void Construct( PlayerConfig config, IPlayerLoopService playerLoopService, IPlatformsService platformsService, IShootingService shootingService )
		{
			_config = config;
			Player  = Object.Instantiate( _config.prefab );

			_playerLoopService = playerLoopService;
			_platformsService  = platformsService;
			
			playerLoopService.OnStarted    += Reset;
			shootingService.OnShoot        += ShootPlayer;
			playerLoopService.OnUpdateTick += HandleDying;
		}

		private void Reset( )
		{
			Player.transformCached.position    = _config.position;
			Player.transformCached.rotation    = _config.rotation;
			Player.rigidbodyCached.velocity    = Vector2.zero;
			Player.rigidbodyCached.isKinematic = false;
		}

		private void ShootPlayer( Vector3 force )
		{
			Player.rigidbodyCached.AddForce( force, ForceMode2D.Impulse );
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