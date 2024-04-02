using System.Collections.Generic;
using Scripts.Distance;
using Scripts.Extensions;
using Scripts.Platforms;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;
using Random = UnityEngine.Random;

namespace Scripts.FallingSteps
{
	public class FallingStepsService : IFallingStepsService
	{
		private FallingStepsConfig          _config;
		private IDistanceService            _distanceService;
		private IPlatformsService           _platformsService;
		private List<FallingStepView>       _steps;
		private ObjectPool<FallingStepView> _pool;

		[Inject]
		private void Construct( FallingStepsConfig config, IDistanceService distanceService, IPlatformsService platformsService )
		{
			_config           = config;
			_distanceService  = distanceService;
			_platformsService = platformsService;

			_steps = new List<FallingStepView>( );
			_pool  = new ObjectPool<FallingStepView>( CreateStep, GetStep, ReleaseStep, DestroyStep, false, _config.poolInitialCount );

			platformsService.OnResetPlatforms += DestroyAll;
			platformsService.OnSpawnPlatform  += TrySpawnSteps;
		}

		private FallingStepView CreateStep( )
		{
			var step = Object.Instantiate( _config.prefab );
			step.Initialize( _pool, _config );
			step.gameObjectCached.SetActive( false );

			return step;
		}

		private void GetStep( FallingStepView step )
		{
			step.ResetProperties( );
			step.gameObjectCached.SetActive( true );
			_steps.Add( step );
		}
		
		private void ReleaseStep( FallingStepView step )
		{
			step.gameObjectCached.SetActive( false );
			_steps.Remove( step );
		}

		
		private void DestroyStep( FallingStepView step )
		{
			_steps.Remove( step );
		}
		
		private void DestroyAll( )
		{
			foreach ( var step in _steps.ToArray( ) )
			{
				_pool.Release( step );
			}
		}

		private void TrySpawnSteps( PlatformView platform )
		{
			var lastPlatform         = platform;
			var lastPlatformPosition = _platformsService.GetStartPosition( lastPlatform ).AddX( -_config.stepDistance );
			if ( _distanceService.GetDifficulty( lastPlatformPosition.x ) < _config.minDifficulty ||
			     _platformsService.Platforms.Count                        < 2 ) return;

			var firstPlatform         = _platformsService.Platforms[^2];
			var firstPlatformPosition = _platformsService.GetEndPosition( firstPlatform ).AddX( _config.stepDistance );
			;

			var distance = Mathf.Abs( lastPlatformPosition.x - firstPlatformPosition.x );

			if ( distance < _config.minDistanceBetweenPlatforms || Random.value > _config.probability ) return;

			var count      = distance / _config.stepDistance;
			var startIndex = Random.Range( 0, _config.maxMissingSteps );
			var endIndex   = count - Random.Range( 0, _config.maxMissingSteps );

			if ( startIndex >= count )
				startIndex = 0;
			if ( endIndex <= startIndex )
				endIndex = count;

			for ( var index = startIndex; index < endIndex; index++ )
			{
				var position     = Vector3.Lerp( firstPlatformPosition, lastPlatformPosition, index         / count );
				var nextPosition = Vector3.Lerp( firstPlatformPosition, lastPlatformPosition, ( index + 1 ) / count );

				var step = _pool.Get( );

				step.transformCached.position = position.AddY( _config.yOffset );
				step.transformCached.rotation = Quaternion.LookRotation( nextPosition - position );

				_steps.Add( step );
			}
		}
	}
}