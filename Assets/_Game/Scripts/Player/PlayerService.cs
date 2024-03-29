using Scripts.Extensions;
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
	
		public void OnHitObstacle( )
		{
			
			_playerLoopService.EndLoop( );
		}

		private PlayerConfig       _config;
		private IPlayerLoopService _playerLoopService;
		private IPlatformsService  _platformsService;
		private Quaternion         _shellRotation;
		private Quaternion         _shellStartRotation;

		[Inject]
		private void Construct( PlayerConfig config, IPlayerLoopService playerLoopService, IPlatformsService platformsService, IShootingService shootingService )
		{
			_config = config;
			Player  = Object.Instantiate( _config.prefab );
			
			Player.Initialize( this );

			_playerLoopService  = playerLoopService;
			_platformsService   = platformsService;
			_shellStartRotation = Player.shellTransform.localRotation;
			
			playerLoopService.OnStarted    += Reset;
			shootingService.OnAim          += UpdatePlayerRotation;
			shootingService.OnShoot        += ShootPlayer;
			playerLoopService.OnUpdateTick += Update;
		}

		private void Reset( )
		{
			Player.ResetValues( );
			
			Player.transformCached.position    = _config.position;
			Player.transformCached.rotation    = _config.rotation;
			Player.rigidbodyCached.velocity    = Vector2.zero;
			Player.rigidbodyCached.isKinematic = false;

			_shellRotation = _shellStartRotation;
		}

		private void UpdatePlayerRotation( Vector3 force )
		{
			if ( force != Vector3.zero )
				_shellRotation = Quaternion.LookRotation( force );
		}

		private void ShootPlayer( Vector3 force )
		{
			Player.rigidbodyCached.AddForce( force, ForceMode2D.Impulse );
		}

		private void Update( )
		{
			HandleRotating( );
			HandleDying( );
		}

		private void HandleRotating( )
		{
			Player.shellTransform.localRotation = Quaternion.Lerp( Player.shellTransform.localRotation, _shellRotation,
				Time.deltaTime * _config.rotationSpeed );
			if ( Player.rigidbodyCached.velocity.sqrMagnitude > 0 )
				_shellRotation = Quaternion.Lerp( _shellRotation, _shellStartRotation, Time.deltaTime * _config.rotationLerpSpeed );
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