using System;
using System.Collections.Generic;
using Scripts.Distance;
using Scripts.Extensions;
using Scripts.Platforms;
using Scripts.PlayerLoop;
using UnityEngine.Pool;
using Zenject;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Scripts.Obstacles
{
	public class ObstaclesService : IObstaclesService
	{
		public event Action<ObstacleView> OnSpawnObstacle   = delegate { };
		public event Action<ObstacleView> OnDestroyObstacle = delegate { };

		private ObstaclesConfig          _config;
		private List<ObstacleView>       _obstacles;
		private ObjectPool<ObstacleView> _pool;
		private IDistanceService         _distanceService;

		private Dictionary<PlatformView, List<ObstacleView>> _obstaclesByPlatform;

		[Inject]
		private void Construct( ObstaclesConfig config, IDistanceService distanceService, IPlayerLoopService playerLoopService, IPlatformsService platformsService )
		{
			_config    = config;
			_obstacles = new List<ObstacleView>( );

			_pool = new ObjectPool<ObstacleView>( CreateObstacle, GetObstacle, ReleaseObstacle, DestroyObstacle, false,
				_config.poolInitialCount );

			_distanceService     = distanceService;
			_obstaclesByPlatform = new Dictionary<PlatformView, List<ObstacleView>>( );

			platformsService.OnResetPlatforms  += Reset;
			platformsService.OnSpawnPlatform   += SpawnObstacles;
			platformsService.OnDestroyPlatform += DestroyObstacles;
		}
		
		private ObstacleView CreateObstacle( )
		{
			return Object.Instantiate( _config.prefabs.GetRandomElement(  ) );;
		}
		
		private void GetObstacle( ObstacleView obstacle )
		{
			obstacle.gameObjectCached.SetActive( true );
			_obstacles.Add( obstacle );
			OnSpawnObstacle.Invoke( obstacle );
		}
		
		private void ReleaseObstacle( ObstacleView obstacle )
		{
			obstacle.gameObjectCached.SetActive( false );
			_obstacles.Remove( obstacle );
			OnDestroyObstacle.Invoke( obstacle );
		}
		
		private void DestroyObstacle( ObstacleView obstacle )
		{
			_obstacles.Remove( obstacle );
			OnDestroyObstacle.Invoke( obstacle );
		}

		private void Reset( )
		{
			foreach ( var obstacle in _obstacles.ToArray( ) )
			{
				_pool.Release( obstacle );
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
				var position = platform.transformCached.position.AddX( xPosition - xSize * 0.5f - platform.colliderCached.offset.x );//* 0.5f + _config.borderSize );
				var obstacle = _pool.Get( );

				obstacle.transformCached.position = position;
				
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
				_pool.Release( obstacle );
			}

			_obstaclesByPlatform[platform].Clear( );
			_obstaclesByPlatform.Remove( platform );
		}
	}
}