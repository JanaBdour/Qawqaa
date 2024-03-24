using UnityEngine;
using Zenject;

namespace Scripts.Platforms
{
	[CreateAssetMenu( fileName = "PlatformsInstaller", menuName = "Installers/PlatformsInstaller" )]

	public class PlatformsInstaller : ScriptableObjectInstaller 
	{
		public PlatformsConfig config;

		public override void InstallBindings( )
		{
			Container.BindInstance( config );
			Container.Bind<IPlatformsService>( ).To<PlatformsService>( ).AsSingle( ).NonLazy( );
		}
	}
}