using System;
using Scripts.Ticking;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace Scripts.Stages
{
    public class StagesService : IStagesService
    {
        private StagesConfig _config;

        public int CurrentStageId
        {
            //TODO Server saving
            get => PlayerPrefs.GetInt( "StageId" );
            set => PlayerPrefs.SetInt( "StageId", value );
        }

        public StageConfig CurrentStageConfig => _config.set.stages[CurrentStageId % _config.set.stages.Length];

        public event Action<int, StageConfig> OnStageStarted  = delegate { };
        public event Action<int>              OnStageFinished = delegate { };


        [Inject]
        private void Construct( StagesConfig config, ITickingService tickingService )
        {
            _config = config;
            
            tickingService.OnStarted += StartStage;
        }

        private void StartStage( )
        {
            OnStageStarted.Invoke( CurrentStageId, CurrentStageConfig );
        }
        
        public void FinishStage( )
        {
            OnStageFinished.Invoke( CurrentStageId );
            CurrentStageId++;
        }
    }
}
