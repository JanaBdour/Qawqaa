using System;
using UnityEngine;
using Zenject;

namespace Scripts.PlayerLoop
{
    public class PlayerLoopService : IPlayerLoopService
    {
        public bool IsPlaying { get; private set; }
        
        public event Action OnUpdateTick = delegate { };
        public event Action OnFixedTick  = delegate { };
        public event Action OnLateTick   = delegate { };
        public event Action OnStarted    = delegate { };
        public event Action OnLoopEnded  = delegate { };

        [Inject]
        private void Construct( )
        {
            new GameObject( "PlayerLoopController" ).AddComponent<PlayerLoopController>( ).Initialize( this );
            IsPlaying = true;
        }

        internal void DispatchUpdateTick( ) => OnUpdateTick.Invoke( );
        internal void DispatchFixedTick( )  => OnFixedTick.Invoke( );
        internal void DispatchLateTick( )   => OnLateTick.Invoke( );
        internal void DispatchStart( )      => OnStarted.Invoke( );

        public void Restart( )
        {
            OnStarted.Invoke( );
            IsPlaying = true;
        }

        public void EndLoop( )
        {
            OnLoopEnded.Invoke( );
            IsPlaying = false;
        }
    }
}