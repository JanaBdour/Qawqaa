using UnityEngine;

namespace Scripts.Stages
{
    [CreateAssetMenu( fileName = "StagesConfig", menuName = "Configs/StagesConfig" )]
    public class StagesConfig : ScriptableObject
    {
        public StagesSet set;
    }
}
