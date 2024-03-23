using System;
using UnityEngine;

namespace Scripts.PlayerLoop
{
    public interface IPlayerLoopService
    {
        event Action OnUpdateTick;
        event Action OnFixedTick;
        event Action OnLateTick;
        event Action OnStarted;
    }
}
