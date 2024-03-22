using UnityEngine;
using Zenject;

namespace Scripts.Ticking
{
    [CreateAssetMenu( fileName = "TickingInstaller", menuName = "Installers/TickingInstaller" )]
    public class TickingInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings( )
        {
            Container.Bind<ITickingService>( ).To<TickingService>( ).AsSingle( ).NonLazy( );
        }
    }
}
