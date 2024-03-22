using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Characters
{
    public interface ICharactersService
    {
        Dictionary<CharacterConfig, List<CharacterView>> Characters { get; }

        CharacterConfig GetConfigFromView( CharacterView view );

        void  ShowCharacter( string                            configName, Vector3   position );
        float AssignTargetAndGetDurationToReach( CharacterView character,  Transform target );
    }
}
