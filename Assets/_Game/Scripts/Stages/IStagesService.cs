using System;
using UnityEngine;

namespace Scripts.Stages
{
    public interface IStagesService
    {
        int CurrentStageId { get; }
        StageConfig CurrentStageConfig { get; }
        
        event Action<int, StageConfig> OnStageStarted;
        event Action<int>              OnStageFinished;

        void FinishStage( );
    }
}
