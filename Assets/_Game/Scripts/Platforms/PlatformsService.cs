using System;
using System.Collections.Generic;
using System.Linq;
using Scripts.Distance;
using Scripts.Extensions;
using Scripts.Player;
using Scripts.PlayerLoop;
using UnityEngine;
using UnityEngine.Profiling;
using Zenject;
using Object = UnityEngine.Object;

namespace Scripts.Platforms
{
	public class PlatformsService : IPlatformsService
	{
		public event Action               OnResetPlatforms  = delegate { };
		public event Action<PlatformView> OnSpawnPlatform   = delegate { };
		public event Action<PlatformView> OnDestroyPlatform = delegate { };
		
		public PlatformView GetLowestPlatform( )
		{
			var lowestY = Mathf.Infinity;
			
			PlatformView lowestPlatform = null;
			foreach ( var platform in _platforms )
			{
				if ( platform.transformCached.position.y >= lowestY ) continue;

				lowestY        = platform.transformCached.position.y;
				lowestPlatform = platform;
			}

			return lowestPlatform;
		}

		public PlatformView GetClosestPlatformOnX( float xPosition )
		{
			var lowestDistance = Mathf.Infinity;
			
			PlatformView closestPlatform = null;
			foreach ( var platform in _platforms )
			{
				var distance = Mathf.Abs( platform.transformCached.position.x - xPosition );
				if ( distance >= lowestDistance ) continue;

				lowestDistance  = distance;
				closestPlatform = platform;
			}

			return closestPlatform;
		}
		
		private PlatformsConfig    _config;
		private List<PlatformView> _platforms;
		private IDistanceService   _distanceService;
		private IPlayerService     _playerService;

		[Inject]
		private void Construct( PlatformsConfig config, IDistanceService distanceService, IPlayerService playerService, IPlayerLoopService playerLoopService )
		{
			_config          = config;
			_platforms       = new List<PlatformView>( );
			_distanceService = distanceService;
			_playerService   = playerService;
			
			playerLoopService.OnStarted    += DestroyAndSpawn;
			playerLoopService.OnUpdateTick += HandleDisappearingAndRespawning;
		}

		private void DestroyAndSpawn( )
		{
			Profiler.BeginSample( "Initial Platform Spawning" );
			foreach ( var platform in _platforms )
			{
				Object.Destroy( platform.gameObjectCached );
			}

			_platforms.Clear( );
			OnResetPlatforms.Invoke( );

			var startPlatform = Object.Instantiate( _config.startPlatformPrefab, Vector3.zero, _config.rotation );
			_platforms.Add( startPlatform );

			for ( var index = 0; index < _config.startCount; index++ )
			{
				SpawnPlatform( );
			}

			Profiler.EndSample( );
		}

		private void HandleDisappearingAndRespawning( )
		{
			foreach ( var platform in _platforms.ToArray( ) )
			{
				var distance = _playerService.Player.transformCached.position.x - platform.transformCached.position.x;
				if ( distance > _config.disappearDistance )
				{
					OnDestroyPlatform.Invoke( platform );
					_platforms.Remove( platform );
					Object.Destroy( platform.gameObjectCached );

					SpawnPlatform( );
				}
			}
		}
		
		private void SpawnPlatform( )
		{
			var lastPlatform = _platforms[^1];

			var platform = Object.Instantiate( _config.platformPrefabs.GetRandomElement( ) );

			var position = lastPlatform.transformCached.position;
				
			var lastCollider    = lastPlatform.colliderCached;
			var currentCollider = platform.colliderCached;
				
			position.x += lastCollider.bounds.size.x    * 0.5f - lastCollider.offset.x;
			position.x += currentCollider.bounds.size.x * 0.5f - currentCollider.offset.x;

			position += Vector3Extensions.GetRandomVector( _config.minDistance, _config.maxDistance ) * _distanceService.GetDifficulty( position.x );
				
			platform.transformCached.position = position;
			platform.transformCached.rotation = _config.rotation;

			_platforms.Add( platform );
			OnSpawnPlatform.Invoke( platform );
		}
	}
}