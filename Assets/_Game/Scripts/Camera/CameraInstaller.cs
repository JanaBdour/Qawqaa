using UnityEngine;
using Zenject;

namespace Scripts.Camera
{
    [CreateAssetMenu( fileName = "CameraInstaller", menuName = "Installers/CameraInstaller" )]
    public class CameraInstaller : ScriptableObjectInstaller
    {
        public CameraConfig config;
        public override void InstallBindings( )
        {
            Container.BindInstance( config );
            Container.Bind<ICameraService>( ).To<CameraService>( ).AsSingle( ).NonLazy( );
        }
    }
}
