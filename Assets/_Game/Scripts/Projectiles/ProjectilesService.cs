using System;
using System.Collections.Generic;
using Scripts.Effects;
using Scripts.Feedback;
using Scripts.Player;
using Scripts.PlayerLoop;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Scripts.Projectiles
{
	public class ProjectilesService : IProjectilesService
	{
		public event Action<Vector3> OnExplode = delegate { };
		
		public void Explode( ProjectileView projectile )
		{
			var position = projectile.transformCached.position;
			_explosion.PlayAtPosition( position );
			Object.Destroy( projectile.gameObjectCached );
			OnExplode.Invoke( position );
		}

		private ProjectilesConfig _config;
		private IPlayerService    _playerService;
		private IFeedbackService  _feedbackService;
		private EffectView        _explosion;

		[Inject]
		private void Construct( ProjectilesConfig config, IPlayerLoopService playerLoopService, IPlayerService playerService, IFeedbackService feedbackService )
		{
			_config          = config;
			_playerService   = playerService;
			_feedbackService = feedbackService;
			_explosion       = Object.Instantiate( _config.effectPrefab );
			
			feedbackService.RegisterAudioSource( _explosion.soundSource );
			
			playerLoopService.OnStarted += StopEffect;
			playerService.OnMove        += ShootNewProjectile;
		}

		private void StopEffect( )
		{
			_explosion.Stop( );
		}

		private void ShootNewProjectile( )
		{
			var projectile = Object.Instantiate( _config.prefab, _playerService.Player.shootPositionTransform.position,
				Random.rotation );
			
			projectile.Initialize( _config, this );
			projectile.rigidbodyCached.AddForce( _config.force );
			projectile.rigidbodyCached.AddTorque( _config.torque );
			_feedbackService.RegisterAudioSource( projectile.shootAudioSource );
		}
	}
}