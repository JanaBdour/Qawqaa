using UnityEngine;

namespace Scripts.Characters
{
    [CreateAssetMenu( fileName = "CharacterConfig", menuName = "Configs/CharacterConfig" )]
    public class CharacterConfig : ScriptableObject
    {
        public CharacterView prefab;
        public int           startCount         = 5;
        public float         durationMultiplier = 0.9f;
    }
}