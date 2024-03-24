using Scripts.PlayerLoop;
using Scripts.Shooting;
using UnityEngine;
using Zenject;

namespace Scripts.Player
{
	public class PlayerService : IPlayerService
	{
		public PlayerView Player { get; private set; }
	
		private PlayerConfig _config;

		[Inject]
		private void Construct( PlayerConfig config, IPlayerLoopService playerLoopService, IShootingService shootingService )
		{
			_config = config;
			Player  = Object.Instantiate( _config.prefab );
			
			playerLoopService.OnStarted += Reset;
			shootingService.OnShoot     += ShootPlayer;
		}

		private void Reset( )
		{
			Player.transformCached.position = _config.position;
			Player.transformCached.rotation = _config.rotation;
			Player.rigidbodyCached.velocity = Vector2.zero;
		}

		private void ShootPlayer( Vector3 force )
		{
			Player.rigidbodyCached.AddForce( force, ForceMode2D.Impulse );
		}
	}
}