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
		public List<PlatformView> Platforms { get; private set; }

		public event Action               OnResetPlatforms  = delegate { };
		public event Action<PlatformView> OnSpawnPlatform   = delegate { };
		public event Action<PlatformView> OnDestroyPlatform = delegate { };
		
		public Vector3 GetStartPosition( PlatformView platform )
		{
			return platform.transformCached.position.AddX( -platform.colliderCached.size.x * 0.5f +
			                                               platform.colliderCached.offset.x );

		}

		public Vector3 GetEndPosition( PlatformView platform )
		{
			return platform.transformCached.position.AddX( platform.colliderCached.size.x * 0.5f +
			                                               platform.colliderCached.offset.x );
		}
		
		public float GetXDistance( PlatformView startPlatform, PlatformView endPlatform )
		{
			return Mathf.Abs( GetStartPosition( endPlatform ).x - GetEndPosition( startPlatform ).x );
		}

		public PlatformView GetClosestPlatformOnX( float xPosition )
		{
			return GetClosestPlatformOnX( xPosition, out var index );
		}

		public PlatformView GetClosestPlatformOnX( float xPosition, out int index )
		{
			var lowestDistance = Mathf.Infinity;
			index = -1;
			
			PlatformView closestPlatform = null;
			for ( var i = 0; i < Platforms.Count; i++ )
			{
				var platform = Platforms[i];
				var distance = Mathf.Abs( platform.transformCached.position.x - xPosition );
				if ( distance >= lowestDistance ) continue;

				index           = i;
				lowestDistance  = distance;
				closestPlatform = platform;
			}

			return closestPlatform;
		}

		private PlatformsConfig  _config;
		private IDistanceService _distanceService;
		private IPlayerService   _playerService;

		[Inject]
		private void Construct( PlatformsConfig config, IDistanceService distanceService, IPlayerService playerService, IPlayerLoopService playerLoopService )
		{
			_config          = config;
			Platforms       = new List<PlatformView>( );
			_distanceService = distanceService;
			_playerService   = playerService;
			
			playerLoopService.OnStarted    += DestroyAndSpawn;
			playerLoopService.OnUpdateTick += HandleDisappearingAndRespawning;
		}

		private void DestroyAndSpawn( )
		{
			Profiler.BeginSample( "Initial Platform Spawning" );
			foreach ( var platform in Platforms )
			{
				Object.Destroy( platform.gameObjectCached );
			}

			Platforms.Clear( );
			OnResetPlatforms.Invoke( );

			var startPlatform = Object.Instantiate( _config.startPlatformPrefab, Vector3.zero, _config.rotation );
			Platforms.Add( startPlatform );

			for ( var index = 0; index < _config.startCount; index++ )
			{
				SpawnPlatform( );
			}

			Profiler.EndSample( );
		}

		private void HandleDisappearingAndRespawning( )
		{
			foreach ( var platform in Platforms.ToArray( ) )
			{
				var distance = _playerService.Player.transformCached.position.x - platform.transformCached.position.x;
				if ( distance > _config.disappearDistance )
				{
					OnDestroyPlatform.Invoke( platform );
					Platforms.Remove( platform );
					Object.Destroy( platform.gameObjectCached );

					SpawnPlatform( );
				}
			}
		}
		
		private void SpawnPlatform( )
		{
			var lastPlatform = Platforms[^1];

			var platform = Object.Instantiate( _config.platformPrefabs.GetRandomElement( ) );

			var position = lastPlatform.transformCached.position;
				
			var lastCollider    = lastPlatform.colliderCached;
			var currentCollider = platform.colliderCached;
				
			position.x += lastCollider.bounds.size.x    * 0.5f - lastCollider.offset.x;
			position.x += currentCollider.bounds.size.x * 0.5f - currentCollider.offset.x;

			position += Vector3Extensions.GetRandomVector( _config.minDistance, _config.maxDistance * _distanceService.GetDifficulty( position.x ) ) ;
				
			platform.transformCached.position = position;
			platform.transformCached.rotation = _config.rotation;

			Platforms.Add( platform );
			OnSpawnPlatform.Invoke( platform );
		}
	}
}