using System;
using UnityEngine;
using Zenject;

namespace Scripts.Ui
{
    [RequireComponent( typeof( CanvasGroup ) )]
    public class Window : MonoBehaviour
    {
        public Action Opened = delegate { }; 
        public Action Closed = delegate { };

        private CanvasGroup _canvasGroup;
        private bool        _isOpened;

        private float _animationTimer = 0f;

        public bool openOnStart = false;

        [Inject] private UiConfig _config;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();

            if ( openOnStart )
                _isOpened = true;
            else
            {
                _canvasGroup.interactable   = false;
                _canvasGroup.blocksRaycasts = false;
                _animationTimer             = 0;
            }
        }

        public bool IsOpened => _isOpened;

        public void Open()
        {
            if ( _isOpened ) return;
            
            _canvasGroup.interactable   = true;
            _canvasGroup.blocksRaycasts = true;
            _isOpened                   = true;
            _animationTimer             = 0f;

            Opened.Invoke();
        }

        public void Open( float delay )
        {
            _isOpened       = true;
            _animationTimer = delay * -_config.windowAnimationSpeed;
        }

        public void Close()
        {
            if ( !_isOpened ) return;
            
            _canvasGroup.interactable   = false;
            _canvasGroup.blocksRaycasts = false;
            _isOpened                   = false;
            _animationTimer             = 1f;

            Closed.Invoke();
        }

        public void Close( float delay )
        {
            Close();
            _animationTimer = 1 + delay * _config.windowAnimationSpeed;
        }

        private void Update()
        {
            _animationTimer    += Time.unscaledDeltaTime * ( _isOpened ? _config.windowAnimationSpeed : -_config.windowAnimationSpeed );
            _canvasGroup.alpha =  Mathf.Lerp( 0f, 1f, _animationTimer );

            if ( _isOpened && !_canvasGroup.interactable && _canvasGroup.alpha == 1f ) {
                _canvasGroup.interactable   = true;
                _canvasGroup.blocksRaycasts = true;
            }

            if ( PlayerPrefs.GetInt( "HUD_Hidden", 0 ) == 1 )
                _canvasGroup.alpha = 0;
        }
    }
}
