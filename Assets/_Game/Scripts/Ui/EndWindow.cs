using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Player;
using Scripts.PlayerLoop;
using UnityEngine;
using Zenject;

namespace Scripts.Ui
{
    [RequireComponent( typeof(Window) )]
    public class EndWindow : MonoBehaviour
    {
        private Window _window;
        
        [Inject] private IPlayerLoopService _playerLoopService;

        private void Awake( )
        {
            _window = GetComponent<Window>( );
            
            _playerLoopService.OnLoopEnded += OpenWindow;
            _playerLoopService.OnStarted   += CloseWindow;
        }

        private void OnDestroy( )
        {
            _playerLoopService.OnLoopEnded -= OpenWindow;
            _playerLoopService.OnStarted   -= CloseWindow;
        }

        private void OpenWindow( )
        {
            _window.Open( );
        }
        
        private void CloseWindow( )
        {
            _window.Close( );
        }
    }
}