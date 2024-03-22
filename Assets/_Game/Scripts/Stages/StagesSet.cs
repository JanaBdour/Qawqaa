using UnityEngine;

namespace Scripts.Stages
{
    [CreateAssetMenu( fileName = "StagesSet", menuName = "Configs/StagesSet" )]
    public class StagesSet : ScriptableObject
    {
        public StageConfig[] stages;
    }
}
