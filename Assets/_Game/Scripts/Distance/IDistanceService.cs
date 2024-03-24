using System;
using UnityEngine;

namespace Scripts.Distance
{
    public interface IDistanceService
    {
        float Distance     { get; }
        float BestDistance { get; }

        event Action OnDistanceUpdated;
        event Action OnBestDistanceUpdated;

        float GetDifficulty( float xPosition );
    }
}