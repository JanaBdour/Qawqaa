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
        private Button _button;

        [Inject] private IPlayerLoopService _playerLoopService;

        private void Awake( )
        {
            _button = GetComponent<Button>( );
            _button.onClick.AddListener( Restart );
        }

        private void OnDestroy( )
        {
            _button.onClick.RemoveListener( Restart );
        }

        private void Restart( )
        {
            _playerLoopService.Restart( );
        }
    }
}