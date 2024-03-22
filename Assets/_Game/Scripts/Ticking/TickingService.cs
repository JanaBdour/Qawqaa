using System;
using UnityEngine;
using Zenject;

namespace Scripts.Ticking
{
    public class TickingService : ITickingService
    {
        public event Action OnUpdateTick = delegate { };
        public event Action OnFixedTick  = delegate { };
        public event Action OnLateTick   = delegate { };
        public event Action OnStarted    = delegate { };

        [Inject]
        private void Construct( )
        {
            new GameObject("Ticking").AddComponent<TickingController>( ).Initialize( this );
        }

        internal void DispatchUpdateTick( ) => OnUpdateTick.Invoke( );
        internal void DispatchFixedTick( )  => OnFixedTick.Invoke( );
        internal void DispatchLateTick( )   => OnLateTick.Invoke( );
        internal void DispatchStart( )      => OnStarted.Invoke( );
    }
}
