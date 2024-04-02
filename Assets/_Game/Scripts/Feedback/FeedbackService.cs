using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Scripts.PlayerLoop;

namespace Scripts.Feedback
{
	public class FeedbackService : IFeedbackService
	{
		public bool IsSoundEnabled
		{
			get => PlayerPrefs.GetInt( "IsSoundEnabled", 1 ) == 1;
			private set => PlayerPrefs.SetInt( "IsSoundEnabled", value ? 1 : 0 );
		}

		public bool IsMusicEnabled
		{
			get => PlayerPrefs.GetInt( "IsMusicEnabled", 1 ) == 1;
			private set => PlayerPrefs.SetInt( "IsMusicEnabled", value ? 1 : 0 );
		}

		public void RegisterAudioSource( AudioSource source )
		{
			if ( !source ) return;

			if ( !_audioSources.Contains( source ) )
				_audioSources.Add( source );
			source.enabled = IsSoundEnabled;
		}

		public void SwitchSound( )
		{
			IsSoundEnabled = !IsSoundEnabled;
			UpdateSounds( );
		}

		public void SwitchMusic( )
		{
			IsMusicEnabled = !IsMusicEnabled;
			UpdateMusic( );
		}

		public void PlayClip( AudioClip clip )
		{
			if ( !clip ) return;
			
			_localSource.clip = clip;
			_localSource.Play( );
		}

		private FeedbackConfig    _config;
		private AudioSource       _localSource;
		private AudioSource       _musicSource;
		private List<AudioSource> _audioSources;

		[Inject]
		private void Construct( FeedbackConfig config )
		{
			_config       = config;
			_localSource  = new GameObject( "FeedbackSource" ).AddComponent<AudioSource>( );
			_musicSource  = Object.FindObjectOfType<MusicPlayer>( ).GetComponent<AudioSource>( );
			_audioSources = new List<AudioSource>( );

			UpdateSounds( );
			UpdateMusic( );
		}
		
		private void UpdateSounds( )
		{
			foreach ( var source in _audioSources )
			{
				if ( source )
					source.enabled = IsSoundEnabled;
			}
		}
		
		private void UpdateMusic( )
		{
			_musicSource.enabled = IsMusicEnabled;
		}
	}
}