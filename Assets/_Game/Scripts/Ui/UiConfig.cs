using UnityEngine;

namespace Scripts.Ui
{
    [CreateAssetMenu( fileName = "UiConfig", menuName = "Configs/UiConfig" )]
    public class UiConfig : ScriptableObject
    {
        public float windowAnimationSpeed = 3f;
    }
}
