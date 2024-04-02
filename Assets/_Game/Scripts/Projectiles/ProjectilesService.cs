using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Scripts.Effects;
using Scripts.Feedback;
using Scripts.Player;
using Scripts.PlayerLoop;
using UnityEngine;
using UnityEngine.Pool;
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

		private ProjectilesConfig          _config;
		private IPlayerService             _playerService;
		private IFeedbackService           _feedbackService;
		private EffectView                 _explosion;
		private ObjectPool<ProjectileView> _pool;

		[Inject]
		private void Construct( ProjectilesConfig config, IPlayerLoopService playerLoopService, IPlayerService playerService, IFeedbackService feedbackService )
		{
			_config          = config;
			_playerService   = playerService;
			_feedbackService = feedbackService;
			_explosion       = Object.Instantiate( _config.effectPrefab );
			
			_pool = new ObjectPool<ProjectileView>( CreateProjectile, GetProjectile, ReleaseProjectile,
				DestroyProjectile, false, _config.poolInitialCount );
			
			feedbackService.RegisterAudioSource( _explosion.soundSource );
			
			playerLoopService.OnStarted += StopEffect;
			playerService.OnMove        += ShootNewProjectile;
		}
		
		private ProjectileView CreateProjectile( )
		{
			var projectile = Object.Instantiate( _config.prefab );
			projectile.gameObjectCached.SetActive( false );
			projectile.Initialize( _config, this );
			_feedbackService.RegisterAudioSource( projectile.shootAudioSource );

			return projectile;
		}

		private void GetProjectile( ProjectileView projectile )
		{
			projectile.gameObjectCached.SetActive( true );
			projectile.ResetProperties( );

			projectile.transformCached.position = _playerService.Player.shootPositionTransform.position;
			projectile.transformCached.rotation = Random.rotation;
			
			projectile.rigidbodyCached.AddForce( _config.force );
		}
		
		private void ReleaseProjectile( ProjectileView projectile )
		{
			projectile.gameObjectCached.SetActive( false );
		}

		private void DestroyProjectile( ProjectileView projectile )
		{
		}

		private void StopEffect( )
		{
			_explosion.Stop( );
		}

		private void ShootNewProjectile( )
		{
			_pool.Get( );
		}
	}
}