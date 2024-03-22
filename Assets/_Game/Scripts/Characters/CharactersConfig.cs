using UnityEngine;

namespace Scripts.Characters
{
    [CreateAssetMenu( fileName = "CharactersConfig", menuName = "Configs/CharactersConfig" )]
    public class CharactersConfig : ScriptableObject
    {
        public CharacterConfig[] characters;

        public float radius       = 4;
        public float stopDistance = 2;
        public float speed        = 5;
        public float yPosition    = 1.13f;
    }
}
