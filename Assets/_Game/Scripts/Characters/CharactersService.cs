using System.Collections.Generic;
using System.Linq;
using Scripts.Ticking;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Scripts.Characters
{
    public class CharactersService : ICharactersService
    {
        private CharactersConfig _config;

        private Dictionary<CharacterView, Transform> _characterTargetMap;
        
        public Dictionary<CharacterConfig, List<CharacterView>> Characters { get; private set; }

        [Inject]
        private void Construct( CharactersConfig config, ITickingService tickingService )
        {
            _config = config;

            _characterTargetMap = new Dictionary<CharacterView, Transform>( );

            Characters = new Dictionary<CharacterConfig, List<CharacterView>>( );
            foreach ( var characterConfig in _config.characters )
            {
                Characters.Add( characterConfig, new List<CharacterView>( ) );
                for ( var index = 0; index < characterConfig.startCount; index++ )
                    InitializeAndGetCharacter( characterConfig );
            }

            tickingService.OnUpdateTick += HandleMovementAndRotation;
        }
        
        CharacterView InitializeAndGetCharacter( CharacterConfig characterConfig )
        {
            var position = _config.radius * Random.insideUnitSphere;
            position.y = 0;

            var character = Object.Instantiate( characterConfig.prefab, position, Quaternion.identity );
            character.gameObjectCached.SetActive( false );

            Characters[characterConfig].Add( character );
            _characterTargetMap.Add( character, null );
            return character;
        }

        private void HandleMovementAndRotation( )
        {
            var step = _config.speed * Time.deltaTime;

            foreach ( var character in _characterTargetMap.Keys )
            {
                var target = _characterTargetMap[character];
                if ( !target ) continue;

                var dist = ( character.transformCached.position - target.position ).sqrMagnitude;
                if ( !character.gameObjectCached.activeSelf || dist < _config.stopDistance * _config.stopDistance ) continue;

                var direction = target.position - character.transformCached.position;
                character.controller.Move( direction * _config.speed * Time.deltaTime );

                var rotation = Quaternion.LookRotation( direction );
                rotation.x = 0;
                rotation.z = 0;

                character.transformCached.rotation = Quaternion.Lerp( character.transformCached.rotation, rotation, step );
            }
        }

        public CharacterConfig GetConfigFromView( CharacterView view )
        {
            return Characters.Keys.First( config => Characters[config].Contains( view ) );
        }

        public float AssignTargetAndGetDurationToReach( CharacterView character, Transform target )
        {
            _characterTargetMap[character] = target;
            return ( ( target.position - character.transform.position ).magnitude - _config.stopDistance ) /
                _config.speed * Time.deltaTime;
        }
        
        public void ShowCharacter( string configName, Vector3 position )
        {
            var config = Characters.Keys.First( conf => conf.name.Equals( configName ) );
            if ( !config ) return;

            var inactiveCharacter = Characters[config].First( character => !character.gameObjectCached.activeSelf );
            if ( !inactiveCharacter )
                inactiveCharacter = InitializeAndGetCharacter( config );

            position.y = _config.yPosition;
            
            inactiveCharacter.transformCached.position = position;
            inactiveCharacter.gameObjectCached.SetActive( true );
        }
    }
}
