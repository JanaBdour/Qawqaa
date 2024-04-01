using System.Collections.Generic;
using Scripts.Distance;
using Scripts.Extensions;
using Scripts.Platforms;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Scripts.FallingSteps
{
	public class FallingStepsService : IFallingStepsService
	{
		private FallingStepsConfig    _config;
		private IDistanceService      _distanceService;
		private IPlatformsService     _platformsService;
		private List<FallingStepView> _steps;

		[Inject]
		private void Construct( FallingStepsConfig config, IDistanceService distanceService, IPlatformsService platformsService )
		{
			_config           = config;
			_distanceService  = distanceService;
			_platformsService = platformsService;

			_steps = new List<FallingStepView>( );

			platformsService.OnResetPlatforms += DestroyAll;
			platformsService.OnSpawnPlatform  += TrySpawnSteps;
		}

		private void DestroyAll( )
		{
			foreach ( var step in _steps )
			{
				Object.Destroy( step.gameObjectCached );
			}

			_steps.Clear( );
		}

		private void TrySpawnSteps( PlatformView platform )
		{
			var lastPlatformPosition = platform.transformCached.position.AddX( -platform.colliderCached.size.x * 0.5f );
			if ( _distanceService.GetDifficulty( lastPlatformPosition.x ) < _config.startDifficulty || _platformsService.Platforms.Count < 2 ) return;

			var firstPlatform         = _platformsService.Platforms[^2];
			var firstPlatformPosition = firstPlatform.transformCached.position.AddX( firstPlatform.colliderCached.size.x * 0.5f );
			var distance              = Mathf.Abs( firstPlatformPosition.x - lastPlatformPosition.x );
		
			if ( distance < _config.minDistanceBetweenPlatforms || Random.value > _config.probability ) return;

			var count = distance / _config.stepDistance;
			for ( int index = 0; index < count; index++ )
			{
				var position     = Vector3.Lerp( firstPlatformPosition, lastPlatformPosition, index     / count );
				var nextPosition = Vector3.Lerp( firstPlatformPosition, lastPlatformPosition, ( index + 1 ) / count );
				var step = Object.Instantiate( _config.prefab, position,
					Quaternion.LookRotation( nextPosition - position ) );
				
				_steps.Add( step );
			}
		}
	}
}