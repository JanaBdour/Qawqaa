using System;
using UnityEngine;

namespace Scripts.PlayerLoop
{
    public class PlayerLoopController : MonoBehaviour
    {
        private PlayerLoopService _playerLoopService;

        public void Initialize( PlayerLoopService playerLoopService ) => _playerLoopService = playerLoopService;

        private void Update( )      => _playerLoopService.DispatchUpdateTick( );
        private void FixedUpdate( ) => _playerLoopService.DispatchFixedTick( );
        private void LateUpdate( )  => _playerLoopService.DispatchLateTick( );
        private void Start( )       => _playerLoopService.DispatchStart( );
    }
}