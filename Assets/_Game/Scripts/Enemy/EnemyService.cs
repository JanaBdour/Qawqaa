using System.Collections.Generic;
using System.Linq;
using Scripts.Extensions;
using Scripts.Obstacles;
using Scripts.PlayerLoop;
using Scripts.Projectiles;
using UnityEngine;
using Zenject;

namespace Scripts.Enemy
{
	public class EnemyService : IEnemyService
	{
		private EnemyConfig _config;
		
		private Dictionary<ObstacleView, EnemyView> _enemies;

		[Inject]
		private void Construct( EnemyConfig config, IPlayerLoopService playerLoopService, IObstaclesService obstaclesService, IProjectilesService projectilesService )
		{
			_config  = config;
			_enemies = new Dictionary<ObstacleView, EnemyView>( );

			projectilesService.OnExplode       += KillEnemy;
			playerLoopService.OnLoopEnded      += ClearEnemies;
			obstaclesService.OnSpawnObstacle   += TryGetEnemy;
			obstaclesService.OnDestroyObstacle += DestroyEnemy;
		}

		private void KillEnemy( Vector3 position )
		{
			foreach ( var obstacle in _enemies.Keys.ToArray( ) )
			{
				CheckObstacle( obstacle );
			}

			void CheckObstacle( ObstacleView obstacle )
			{
				var enemy    = _enemies[obstacle];
				var distance = ( enemy.meshTransform.position - position ).sqrMagnitude;

				if ( distance <= _config.killDistance )
				{
					enemy.Die( );
					_enemies.Remove( obstacle );
				}
			}
		}

		private void ClearEnemies( )
		{
			_enemies.Clear( );
		}

		private void TryGetEnemy( ObstacleView obstacle )
		{
			if ( obstacle.name.Contains( "Enemy" ) && obstacle.TryGetComponent( out EnemyView enemy ) )
			{
				_enemies.Add( obstacle, enemy );
				enemy.Initialize( _config );
			}
		}

		private void DestroyEnemy( ObstacleView obstacle )
		{
			if ( _enemies.ContainsKey( obstacle ) )
				_enemies.Remove( obstacle );
		}
	}
}