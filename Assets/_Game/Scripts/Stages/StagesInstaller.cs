using UnityEngine;
using Zenject;

namespace Scripts.Stages
{
    [CreateAssetMenu( fileName = "StagesInstaller", menuName = "Installers/StagesInstaller" )]
    public class StagesInstaller : ScriptableObjectInstaller
    {
        public StagesConfig config;

        public override void InstallBindings( )
        {
            Container.BindInstance( config );
            Container.Bind<IStagesService>( ).To<StagesService>( ).AsSingle( ).NonLazy( );
        }
    }
}