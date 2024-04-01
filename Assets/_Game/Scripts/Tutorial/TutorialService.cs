using System;
using Scripts.Distance;
using Scripts.Enemy;
using Scripts.Platforms;
using Scripts.Player;
using Scripts.PlayerLoop;
using UnityEngine;
using Zenject;

namespace Scripts.Tutorial
{
	public class TutorialService : ITutorialService
	{
		public event Action<string> OnShowText = delegate { };

		private TutorialConfig    _config;
		private IPlatformsService _platformsService;
		private IPlayerService    _playerService;

		private int    _lastIndex;
		private bool   _hasStarted;
		private bool   _isWaitingForLongMove;
		private float  _waitTimer;
		private int    _moveCount;
		private int    _killCount;
		private string _tapText = Application.isMobilePlatform ? "Tap" : "Click";


		private bool IsWholeTutorialCompleted
		{
			get => PlayerPrefs.GetInt( "IsWholeTutorialCompleted" ) == 1;
			set => PlayerPrefs.SetInt( "IsWholeTutorialCompleted", value ? 1 : 0 );
		}

		private bool IsMoveTutorialCompleted
		{
			get => PlayerPrefs.GetInt( "IsMoveTutorialCompleted" ) == 1;
			set => PlayerPrefs.SetInt( "IsMoveTutorialCompleted", value ? 1 : 0 );
		}

		private bool IsLongMoveTutorialCompleted
		{
			get => PlayerPrefs.GetInt( "IsLongMoveTutorialCompleted" ) == 1;
			set => PlayerPrefs.SetInt( "IsLongMoveTutorialCompleted", value ? 1 : 0 );
		}

		[Inject]
		private void Construct( TutorialConfig config, IPlatformsService platformsService, IPlayerService playerService,
			IPlayerLoopService playerLoopService )
		{
			_config = config;

			_platformsService = platformsService;
			_playerService    = playerService;

			playerLoopService.OnStarted    += WaitToShowTutorial;
			playerLoopService.OnUpdateTick += Update;
			playerService.OnMove           += DecreaseMoveCount;
			playerService.OnLongMove       += FinishLongMoveTutorial;
		}

		private void WaitToShowTutorial( )
		{
			if ( IsWholeTutorialCompleted ) return;

			_waitTimer = _config.waitToShowTutorial;
		}

		private void Update( )
		{
			HandleStartWaiting( );
			HandleLongMoveWaiting( );
		}

		private void ShowTutorial( )
		{
			if ( IsWholeTutorialCompleted ) return;

			_hasStarted = true;


			if ( !IsMoveTutorialCompleted )
			{
				_moveCount = _config.passMoveCount;
				OnShowText.Invoke( $"{_tapText}!" );
			}
			else if ( !IsLongMoveTutorialCompleted )
			{
				_isWaitingForLongMove = true;
				OnShowText.Invoke( string.Empty );
			}
			else
			{
				OnShowText.Invoke( string.Empty );
				IsWholeTutorialCompleted = true;
			}
		}

		private void DecreaseMoveCount( )
		{
			if ( IsMoveTutorialCompleted ) return;

			_moveCount--;

			if ( _moveCount > 0 ) return;

			IsMoveTutorialCompleted = true;
			ShowTutorial( );
		}

		private void FinishLongMoveTutorial( )
		{
			IsLongMoveTutorialCompleted = true;
		}

		private void HandleStartWaiting( )
		{
			if ( IsWholeTutorialCompleted || _hasStarted ) return;

			_waitTimer -= Time.deltaTime;
			if ( _waitTimer <= 0 )
			{
				_hasStarted = true;
				ShowTutorial( );
			}
		}

		private void HandleLongMoveWaiting( )
		{
			if ( IsWholeTutorialCompleted || !_hasStarted || IsLongMoveTutorialCompleted ||
			     !_isWaitingForLongMove ) return;

			var platform = _platformsService.GetClosestPlatformOnX( _playerService.Player.transformCached.position.x, out var index );
			if ( _lastIndex == index )
				return;

			_lastIndex = index;
			
			var xDistance = _platformsService.GetXDistance( platform, _platformsService.Platforms[index + 1] );

			if ( xDistance >= _config.longMoveDistance )
			{
				_isWaitingForLongMove = false;
				OnShowText.Invoke( $"{_tapText} and hold to move further!" );
			}
		}
	}
}