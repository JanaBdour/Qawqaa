using System;
using UnityEngine;

namespace Scripts.Ticking
{
    public interface ITickingService
    {
        event Action OnUpdateTick;
        event Action OnFixedTick;
        event Action OnLateTick;
        event Action OnStarted;
    }
}
