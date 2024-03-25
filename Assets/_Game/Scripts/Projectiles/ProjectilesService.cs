using System.Collections.Generic;
using Scripts.Player;
using Scripts.PlayerLoop;
using Scripts.Shooting;
using UnityEngine;
using Zenject;

namespace Scripts.Projectiles
{
	public class ProjectilesService : IProjectilesService
	{
		public void Explode( ProjectileView projectile )
		{
			Object.Destroy( projectile.gameObjectCached );
		}

		private ProjectilesConfig _config;
		private IPlayerService    _playerService;

		[Inject]
		private void Construct( ProjectilesConfig config, IShootingService shootingService, IPlayerService playerService )
		{
			_config        = config;
			_playerService = playerService;
			
			shootingService.OnShoot += ShootNewProjectile;
		}

		private void ShootNewProjectile( Vector3 force )
		{
			var projectile = Object.Instantiate( _config.prefab, _playerService.Player.shootPositionTransform.position,
				Random.rotation );
			
			projectile.Initialize( _config, this );
			projectile.rigidbodyCached.AddForce( force * _config.forceMultiplier, ForceMode2D.Impulse );
			projectile.rigidbodyCached.AddTorque( _config.torque, ForceMode2D.Impulse );
		}
	}
}