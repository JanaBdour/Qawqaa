using System;
using System.Collections.Generic;
using Scripts.Effects;
using Scripts.Extensions;
using Scripts.Feedback;
using Scripts.Obstacles;
using Scripts.Platforms;
using Scripts.Player;
using Scripts.PlayerLoop;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Scripts.Collectible
{
	public class CollectibleService : ICollectibleService
	{
		public int CollectedCount
		{
			get => PlayerPrefs.GetInt( "CollectedCount" );
			private set => PlayerPrefs.SetInt( "CollectedCount", value );
		}
		
		public event Action OnUpdateCount = delegate { };
		
		private CollectibleConfig     _config;
		private List<CollectibleView> _views;
		private EffectView            _collectEffect;
		private IPlayerService        _playerService;
		
		private Dictionary<PlatformView, List<CollectibleView>> _collectiblesByPlatform;

		[Inject]
		private void Construct( CollectibleConfig config, IPlayerService playerService, IPlatformsService platformsService, IFeedbackService feedbackService, IPlayerLoopService playerLoopService )
		{
			_config = config;
			_views  = new List<CollectibleView>( );

			_collectEffect          = Object.Instantiate( _config.collectEffectPrefab );
			_playerService          = playerService;
			_collectiblesByPlatform = new Dictionary<PlatformView, List<CollectibleView>>( );
			
			feedbackService.RegisterAudioSource( _collectEffect.soundSource );
			
			platformsService.OnResetPlatforms  += ClearAll;
			platformsService.OnSpawnPlatform   += SpawnCollectibles;
			platformsService.OnDestroyPlatform += DestroyCollectibles;
			playerLoopService.OnUpdateTick     += HandleCollecting;
		}

		private void ClearAll( )
		{
			foreach ( var view in _views )
			{
				if ( view )
					Object.Destroy( view.gameObjectCached );
			}

			_views.Clear( );
			_collectiblesByPlatform.Clear( );
		}

		private void SpawnCollectibles( PlatformView platform )
		{
			Random.InitState( (int) DateTime.Now.Ticks );

			var count = Random.Range( _config.minCountPerPlatform, _config.maxCountPerPlatform );
			
			var position = platform.transformCached.position + _config.offset;
			var xSize    = platform.colliderCached.size.x * 0.5f;

			for ( var index = 0; index < count; index++ )
			{
				var newPosition = position.AddX( Random.Range( -xSize, xSize ) );
				var collectible = Object.Instantiate( _config.prefab, newPosition, Quaternion.identity );
				
				collectible.Initialize( _config );
				
				_views.Add( collectible );
				if ( _collectiblesByPlatform.ContainsKey( platform ) )
					_collectiblesByPlatform[platform].Add( collectible );
				else
					_collectiblesByPlatform.Add( platform, new List<CollectibleView>( ) {collectible} );
			}
		}
		
		private void DestroyCollectibles( PlatformView platform )
		{
			if ( !_collectiblesByPlatform.ContainsKey( platform ) ) return;

			foreach ( var collectible in _collectiblesByPlatform[platform] )
			{
				if ( collectible )
					Object.Destroy( collectible.gameObjectCached );
			}

			_collectiblesByPlatform[platform].Clear( );
		}

		private void HandleCollecting( )
		{
			var playerPosition = _playerService.Player.transformCached.position;
			foreach ( var view in _views )
			{
				if ( !view ) continue;

				var position = view.transformCached.position;
				var distance = ( position - playerPosition ).sqrMagnitude;
				
				if ( distance > _config.collectDistance ) continue;

				_collectEffect.PlayAtPosition( position );
				
				_views.Remove( view );
				Object.Destroy( view.gameObjectCached );
				CollectedCount++;
				OnUpdateCount.Invoke( );
				break;
			}
		}
	}
}