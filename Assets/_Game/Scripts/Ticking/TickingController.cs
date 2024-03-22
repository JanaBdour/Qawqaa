using System;
using UnityEngine;

namespace Scripts.Ticking
{
    public class TickingController : MonoBehaviour
    {
        private TickingService _tickingService;

        public void Initialize( TickingService tickingService ) => _tickingService = tickingService;

        private void Update( )      => _tickingService.DispatchUpdateTick( );
        private void FixedUpdate( ) => _tickingService.DispatchFixedTick( );
        private void LateUpdate( )  => _tickingService.DispatchLateTick( );
        private void Start( )       => _tickingService.DispatchStart( );
    }
}