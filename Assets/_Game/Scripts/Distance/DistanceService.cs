using System;
using Scripts.Player;
using Scripts.PlayerLoop;
using UnityEngine;
using Zenject;

namespace Scripts.Distance
{
	public class DistanceService : IDistanceService
	{
		public float Distance   { get; private set; }
		public float BestDistance
		{
			get => PlayerPrefs.GetFloat( "BestDistance" );
			private set => PlayerPrefs.SetFloat( "BestDistance", value );
		}

		public event Action OnDistanceUpdated     = delegate { };
		public event Action OnBestDistanceUpdated = delegate { };
	
		public float GetDifficulty( float xPosition )
		{
			return Mathf.Clamp01( xPosition / _config.maxDifficultyDistance );
		}

		private DistanceConfig _config;
		private IPlayerService _playerService;

		[Inject]
		private void Construct( DistanceConfig config, IPlayerService playerService, IPlayerLoopService playerLoop )
		{
			_config        = config;
			_playerService = playerService;
			
			playerLoop.OnStarted += Reset;
			playerLoop.OnUpdateTick += UpdateDistance;
			playerLoop.OnLoopEnded  += UpdateBestDistance;
		}

		private void Reset( )
		{
			Distance = 0;
			OnDistanceUpdated.Invoke( );
		}

		private void UpdateDistance( )
		{
			var distance = Distance;
			
			Distance = Mathf.Round( _playerService.Player.transformCached.position.x );

			if ( Math.Abs( distance - Distance ) > _config.newDistanceTolerance )
			{
				OnDistanceUpdated.Invoke( );
			}
		}

		private void UpdateBestDistance( )
		{
			if ( BestDistance >= Distance ) return;
			
			BestDistance = Distance;
			OnBestDistanceUpdated.Invoke( );
		}
	}
}