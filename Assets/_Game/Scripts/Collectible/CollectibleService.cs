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
using UnityEngine.Pool;
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
		
		private CollectibleConfig           _config;
		private List<CollectibleView>       _views;
		private ObjectPool<CollectibleView> _pool;

		private EffectView        _collectEffect;
		private IPlayerService    _playerService;
		private IPlatformsService _platformsService;
		
		private Dictionary<PlatformView, List<CollectibleView>> _collectiblesByPlatform;

		[Inject]
		private void Construct( CollectibleConfig config, IPlayerService playerService, IPlatformsService platformsService, IFeedbackService feedbackService, IPlayerLoopService playerLoopService )
		{
			_config = config;
			_views  = new List<CollectibleView>( );
			_pool   = new ObjectPool<CollectibleView>( CreateCollectible, GetCollectible, ReleaseCollectible, DestroyCollectible, false, _config.poolInitialCount );

			_collectEffect          = Object.Instantiate( _config.collectEffectPrefab );
			_playerService          = playerService;
			_platformsService       = platformsService;
			_collectiblesByPlatform = new Dictionary<PlatformView, List<CollectibleView>>( );
			
			feedbackService.RegisterAudioSource( _collectEffect.soundSource );
			
			platformsService.OnResetPlatforms  += ClearAll;
			platformsService.OnSpawnPlatform   += SpawnCollectibles;
			platformsService.OnDestroyPlatform += DestroyCollectibles;
			playerLoopService.OnUpdateTick     += HandleCollecting;
		}

		private CollectibleView CreateCollectible( )
		{
			var collectible = Object.Instantiate( _config.prefab );
			collectible.gameObjectCached.SetActive( false );
			collectible.Initialize( _config );

			return collectible;
		}

		private void GetCollectible( CollectibleView collectible )
		{
			collectible.gameObjectCached.SetActive( true );
			_views.Add( collectible );
		}
		
		private void ReleaseCollectible( CollectibleView collectible )
		{
			collectible.gameObjectCached.SetActive( false );
			_views.Remove( collectible );
		}

		private void DestroyCollectible( CollectibleView collectible )
		{
			_views.Remove( collectible );
		}
		
		private void ClearAll( )
		{
			foreach ( var view in _views.ToArray( ) )
			{
				if ( view )
					_pool.Release( view );
			}

			_views.Clear( );
			_collectiblesByPlatform.Clear( );
		}

		private void SpawnCollectibles( PlatformView platform )
		{
			Random.InitState( (int) DateTime.Now.Ticks );

			var count = Random.Range( _config.minCountPerPlatform, _config.maxCountPerPlatform );

			var startPosition = _platformsService.GetStartPosition( platform );
			var endPosition   = _platformsService.GetEndPosition( platform );

			for ( var index = 0; index < count; index++ )
			{
				var position = Vector3Extensions.GetRandomVector( startPosition, endPosition ) + _config.offset;
				var collectible = _pool.Get( );

				collectible.transformCached.position = position; 
				collectible.ResetProperties( );

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
					_pool.Release( collectible );
			}

			_collectiblesByPlatform[platform].Clear( );
		}

		private void HandleCollecting( )
		{
			var playerPosition = _playerService.Player.transformCached.position;
			foreach ( var view in _views )
			{
				if ( !view || !view.gameObjectCached.activeSelf ) continue;

				var position = view.transformCached.position;
				var distance = ( position - playerPosition ).sqrMagnitude;
				
				if ( distance > _config.collectDistance ) continue;

				_collectEffect.PlayAtPosition( position );
				
				_pool.Release( view );

				CollectedCount++;
				OnUpdateCount.Invoke( );
				break;
			}
		}
	}
}