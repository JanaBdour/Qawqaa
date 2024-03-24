using System.Collections;
using System.Collections.Generic;
using Scripts.PlayerLoop;
using UnityEngine;
using Zenject;

namespace Scripts.Ui
{
    [RequireComponent( typeof(Window) )]
    public class MainWindow : MonoBehaviour
    {
        private Window _window;

        [Inject] private IPlayerLoopService _playerLoopService;

        private void Awake( )
        {
            _window = GetComponent<Window>( );

            _playerLoopService.OnStarted   += OpenWindow;
            _playerLoopService.OnLoopEnded += CloseWindow;
        }

        private void OnDestroy( )
        {
            _playerLoopService.OnStarted   -= OpenWindow;
            _playerLoopService.OnLoopEnded -= CloseWindow;
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