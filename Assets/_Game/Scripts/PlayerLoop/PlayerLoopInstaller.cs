using UnityEngine;
using Zenject;

namespace Scripts.PlayerLoop
{
    [CreateAssetMenu( fileName = "PlayerLoopInstaller", menuName = "Installers/PlayerLoopInstaller" )]
    public class PlayerLoopInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings( )
        {
            Container.Bind<IPlayerLoopService>( ).To<PlayerLoopService>( ).AsSingle( ).NonLazy( );
        }
    }
}
