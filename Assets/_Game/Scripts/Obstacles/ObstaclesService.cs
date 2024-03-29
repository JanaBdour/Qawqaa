using System.Collections.Generic;
using Scripts.Distance;
using Scripts.Extensions;
using Scripts.Platforms;
using Scripts.PlayerLoop;
using UnityEngine;
using Zenject;

namespace Scripts.Obstacles
{
	public class ObstaclesService : IObstaclesService
	{
		private ObstaclesConfig    _config;
		private List<ObstacleView> _obstacles;
		private IDistanceService   _distanceService;

		private Dictionary<PlatformView, List<ObstacleView>> _obstaclesByPlatform;

		[Inject]
		private void Construct( ObstaclesConfig config, IDistanceService distanceService, IPlayerLoopService playerLoopService, IPlatformsService platformsService )
		{
			_config    = config;
			_obstacles = new List<ObstacleView>( );

			_distanceService     = distanceService;
			_obstaclesByPlatform = new Dictionary<PlatformView, List<ObstacleView>>( );

			platformsService.OnResetPlatforms  += Reset;
			platformsService.OnSpawnPlatform   += SpawnObstacles;
			platformsService.OnDestroyPlatform += DestroyObstacles;
		}

		private void Reset( )
		{
			foreach ( var obstacle in _obstacles )
			{
				Object.Destroy( obstacle.gameObjectCached );
			}

			_obstacles.Clear( );
			_obstaclesByPlatform.Clear( );
		}

		private void SpawnObstacles( PlatformView platform )
		{
			var xSize     = platform.colliderCached.size.x - _config.borderSize * 2;
			var maxCount  = xSize / _config.maxDistance;
			var xPosition = 0f;
			for ( var index = 0; index < maxCount; index++ )
			{
				xPosition += Random.Range( _config.minDistance, _config.maxDistance );
				if ( xPosition >= xSize - _config.borderSize || Random.value > _config.probability ) continue;
				
				SpawnObstacle( );
			}

			void SpawnObstacle( )
			{
				var prefab   = _config.prefabs[Random.Range( 0, _config.prefabs.Length )];
				var position = platform.transformCached.position.AddX( xPosition - xSize * 0.5f - platform.colliderCached.offset.x );//* 0.5f + _config.borderSize );

				if ( prefab.difficulty > _distanceService.GetDifficulty( position.x ) ) return;

				var obstacle = Object.Instantiate( prefab, position, prefab.transformCached.rotation );

				_obstacles.Add( obstacle );
				if ( _obstaclesByPlatform.ContainsKey( platform ) )
					_obstaclesByPlatform[platform].Add( obstacle );
				else
					_obstaclesByPlatform.Add( platform, new List<ObstacleView> {obstacle} );
			}
		}

		private void DestroyObstacles( PlatformView platform )
		{
			if ( !_obstaclesByPlatform.ContainsKey( platform ) ) return;

			foreach ( var obstacle in _obstaclesByPlatform[platform] )
			{
				_obstacles.Remove( obstacle );
				Object.Destroy( obstacle.gameObjectCached );
			}

			_obstaclesByPlatform[platform].Clear( );
			_obstaclesByPlatform.Remove( platform );
		}
	}
}