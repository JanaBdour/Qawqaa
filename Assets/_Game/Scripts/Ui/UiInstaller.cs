using UnityEngine;
using Zenject;

namespace Scripts.Ui
{
    [CreateAssetMenu( fileName = "UiInstaller", menuName = "Installers/UiInstaller" )]
    public class UiInstaller : ScriptableObjectInstaller
    {
        public UiConfig config;

        public override void InstallBindings( )
        {
            Container.BindInstance( config );
            Container.Bind<IUiService>( ).To<UiService>( ).AsSingle( ).NonLazy( );
        }
    }
}
