using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

namespace Scripts.Tutorial
{
	public class TutorialTextView : MonoBehaviour
	{
		private TextMeshProUGUI _textMesh;
		private RectTransform   _transform;

		[Inject] private TutorialConfig   _config;
		[Inject] private ITutorialService _service;
		
		private void Awake( )
		{
			_textMesh  = GetComponent<TextMeshProUGUI>( );
			_transform = GetComponent<RectTransform>( );

			ShowText( string.Empty );

			_service.OnShowText += ShowText;
		}

		private void OnDestroy( )
		{
			_service.OnShowText -= ShowText;
		}

		private void ShowText( string text )
		{
			_textMesh.SetText( text );

			if ( text.Length == 0 ) return;
			
			_transform.DOScale( _config.pulseScale, _config.pulseDuration ).OnComplete( ( ) =>
				_transform.DOScale( 1, _config.pulseDuration ) );
		}
	}
}