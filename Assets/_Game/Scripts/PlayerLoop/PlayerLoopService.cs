using System;
using UnityEngine;
using Zenject;

namespace Scripts.PlayerLoop
{
    public class PlayerLoopService : IPlayerLoopService
    {
        public event Action OnUpdateTick = delegate { };
        public event Action OnFixedTick  = delegate { };
        public event Action OnLateTick   = delegate { };
        public event Action OnStarted    = delegate { };

        [Inject]
        private void Construct( )
        {
            new GameObject( "PlayerLoopController" ).AddComponent<PlayerLoopController>( ).Initialize( this );
        }

        internal void DispatchUpdateTick( ) => OnUpdateTick.Invoke( );
        internal void DispatchFixedTick( )  => OnFixedTick.Invoke( );
        internal void DispatchLateTick( )   => OnLateTick.Invoke( );
        internal void DispatchStart( )      => OnStarted.Invoke( );
    }
}