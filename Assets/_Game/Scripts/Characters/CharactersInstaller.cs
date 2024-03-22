using UnityEngine;
using Zenject;

namespace Scripts.Characters
{
    [CreateAssetMenu( fileName = "CharactersInstaller", menuName = "Installers/CharactersInstaller" )]
    public class CharactersInstaller : ScriptableObjectInstaller
    {
        public CharactersConfig config;

        public override void InstallBindings( )
        {
            Container.BindInstance( config );
            Container.Bind<ICharactersService>( ).To<CharactersService>( ).AsSingle( ).NonLazy( );
        }
    }
}
