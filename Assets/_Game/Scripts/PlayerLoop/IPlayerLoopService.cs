using System;
using UnityEngine;

namespace Scripts.PlayerLoop
{
    public interface IPlayerLoopService
    {
        bool IsPlaying { get; }
        
        event Action OnUpdateTick;
        event Action OnFixedTick;
        event Action OnLateTick;
        event Action OnStarted;
        event Action OnLoopEnded;

        void Restart( );
        void EndLoop( );
    }
}
