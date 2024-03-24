using System.Collections.Generic;
using Scripts.Extensions;
using Scripts.PlayerLoop;
using UnityEngine;
using UnityEngine.Profiling;
using Zenject;

namespace Scripts.Platforms
{
	public class PlatformsService : IPlatformsService
	{
		private PlatformsConfig    _config;
		private List<PlatformView> _platforms;

		[Inject]
		private void Construct( PlatformsConfig config, IPlayerLoopService playerLoopService )
		{
			_config    = config;
			_platforms = new List<PlatformView>( );

			playerLoopService.OnStarted += DestroyAndSpawn;
		}

		private void DestroyAndSpawn( )
		{
			Profiler.BeginSample( "Platform Spawning" );
			foreach ( var platform in _platforms )
			{
				Object.Destroy( platform.gameObjectCached );
			}

			_platforms.Clear( );

			var startPlatform = Object.Instantiate( _config.startPlatformPrefab, Vector3.zero, _config.rotation );
			_platforms.Add( startPlatform );

			for ( var index = 0; index < _config.startCount; index++ )
			{
				SpawnPlatform( );
			}

			void SpawnPlatform( )
			{
				var lastPlatform = _platforms[^1];

				var platform = Object.Instantiate( _config.platformPrefabs.GetRandomElement( ) );

				var position = lastPlatform.transformCached.position;
				
				var lastCollider    = lastPlatform.colliderCached;
				var currentCollider = platform.colliderCached;
				
				position.x += lastCollider.bounds.size.x    * 0.5f - lastCollider.offset.x;
				position.x += currentCollider.bounds.size.x * 0.5f - currentCollider.offset.x;

				platform.transformCached.position = position;
				platform.transformCached.rotation = _config.rotation;

				_platforms.Add( platform );
			}
			
			Profiler.EndSample( );
		}
	}
}