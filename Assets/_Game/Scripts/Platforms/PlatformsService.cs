using System;
using System.Collections.Generic;
using System.Linq;
using Scripts.Distance;
using Scripts.Extensions;
using Scripts.Player;
using Scripts.PlayerLoop;
using UnityEngine;
using UnityEngine.Pool;
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

		private PlatformsConfig          _config;
		private IDistanceService         _distanceService;
		private IPlayerService           _playerService;
		private PlatformView             _startPlatform;
		private ObjectPool<PlatformView> _pool;

		[Inject]
		private void Construct( PlatformsConfig config, IDistanceService distanceService, IPlayerService playerService, IPlayerLoopService playerLoopService )
		{
			_config          = config;
			Platforms       = new List<PlatformView>( );
			_distanceService = distanceService;
			_playerService   = playerService;
			
			_startPlatform = Object.Instantiate( _config.startPlatformPrefab, Vector3.zero, _config.rotation );

			_pool = new ObjectPool<PlatformView>( CreatePlatform, GetPlatform, ReleasePlatform, DestroyPlatform, false, _config.poolInitialCount, _config.poolInitialCount );
			
			playerLoopService.OnStarted    += DestroyAndSpawn;
			playerLoopService.OnUpdateTick += HandleDisappearingAndRespawning;
		}

		private PlatformView CreatePlatform( )
		{
			var platform = Object.Instantiate( _config.platformPrefabs.GetRandomElement( ) );
			return platform;
		}
		
		private void GetPlatform( PlatformView platform )
		{
			platform.gameObjectCached.SetActive( true );
			
			var lastPlatform = Platforms[^1];

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
		
		private void ReleasePlatform( PlatformView platform )
		{
			platform.gameObjectCached.SetActive( false );
		}
		
		private void DestroyPlatform( PlatformView platform )
		{
			Platforms.Remove( platform );
			OnDestroyPlatform.Invoke( platform );
		}

		private void DestroyAndSpawn( )
		{
			foreach ( var platform in Platforms )
			{
				_pool.Release( platform );
			}

			Platforms.Clear( );
			OnResetPlatforms.Invoke( );
			
			Platforms.Add( _startPlatform );
			_startPlatform.gameObjectCached.SetActive( true );

			for ( var index = 0; index < _config.startCount; index++ )
			{
				_pool.Get( );
			}
		}

		private void HandleDisappearingAndRespawning( )
		{
			var platformsArray = Platforms.ToArray( );
			for ( var index = 1; index < platformsArray.Length; index++ )
			{
				var platform = platformsArray[index];
				var distance = _playerService.Player.transformCached.position.x - platform.transformCached.position.x;
				if ( distance > _config.disappearDistance )
				{
					_pool.Release( platform );
					Platforms.Remove( platform );
					OnDestroyPlatform.Invoke( platform );

					_pool.Get( );
				}
			}
		}
	}
}