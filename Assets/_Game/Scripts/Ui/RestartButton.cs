using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.PlayerLoop;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Scripts.Ui
{
    [RequireComponent( typeof(Button) )]
    public class RestartButton : MonoBehaviour
    {
        [SerializeField] private float interactWait = 0.5f;

        private Button _button;
        private Window _window;
        private float  _timer;

        [Inject] private IPlayerLoopService _playerLoopService;

        private void Awake( )
        {
            _button = GetComponent<Button>( );
            _window = GetComponentInParent<Window>( );

            _window.Opened += WaitToEnableButton;
            _button.onClick.AddListener( Restart );
        }

        private void OnDestroy( )
        {
            _window.Opened -= WaitToEnableButton;
            _button.onClick.RemoveListener( Restart );
        }

        private void WaitToEnableButton( )
        {
            _timer = interactWait;

            _button.interactable = false;
        }

        private void Update( )
        {
            if ( !_window.IsOpened || _button.interactable ) return;

            _timer -= Time.deltaTime;
            if ( _timer <= 0 )
                _button.interactable = true;
        }

        private void Restart( )
        {
            _playerLoopService.Restart( );
        }
    }
}