using UnityEngine;

namespace Scripts.Feedback
{
	public interface IFeedbackService 
	{
		bool IsSoundEnabled { get; }
		bool IsMusicEnabled { get; }

		void RegisterAudioSource( AudioSource source );
		void SwitchSound( );
		void SwitchMusic( );
		void PlayClip( AudioClip clip );
	}
}