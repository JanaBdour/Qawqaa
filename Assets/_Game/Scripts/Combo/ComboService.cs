using System;
using Scripts.Enemy;
using Scripts.PlayerLoop;
using UnityEngine;
using Zenject;

namespace Scripts.Combo
{
	public class ComboService : IComboService
	{
		public float Progress { get; private set; }
		public bool  IsCombo  { get; private set; }

		public float ComboProgress => 1 - ( _comboTimer / _config.comboDuration );

		public event Action OnStartCombo = delegate { };
		public event Action OnEndCombo   = delegate { };
		
		private ComboConfig _config;
		private float       _comboTimer;

		[Inject]
		private void Construct( ComboConfig config, IEnemyService enemyService, IPlayerLoopService playerLoopService )
		{
			_config = config;

			enemyService.OnKillEnemy       += IncreaseProgress;
			playerLoopService.OnStarted    += Reset;
			playerLoopService.OnLoopEnded  += Reset;
			playerLoopService.OnUpdateTick += HandleDecreasing;
		}

		private void IncreaseProgress( )
		{
			if ( IsCombo ) return;
			
			Progress += _config.killProgressIncrease;

			if ( Progress >= 1 )
			{
				_comboTimer = _config.comboDuration;
				IsCombo     = true;

				OnStartCombo.Invoke( );
			}
		}

		private void Reset( )
		{
			Progress    = 0;
			_comboTimer = 0;
			IsCombo     = false;
		}

		private void HandleDecreasing( )
		{
			if ( Progress > 0 && Progress < 1 )
				Progress -= _config.decreasingSpeed * Time.deltaTime;
			
			if ( IsCombo && _comboTimer > 0 )
			{
				_comboTimer -= Time.deltaTime;

				if ( _comboTimer <= 0 )
				{
					Progress = 0;
					IsCombo  = false;
					OnEndCombo.Invoke( );
				}
			}
		}
	}
}