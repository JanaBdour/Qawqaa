using UnityEngine;
using Zenject;

namespace Scripts.Shooting
{
	[CreateAssetMenu( fileName = "ShootingInstaller", menuName = "Installers/ShootingInstaller" )]

	public class ShootingInstaller : ScriptableObjectInstaller 
	{
		public ShootingConfig config;

		public override void InstallBindings( )
		{
			Container.BindInstance( config );
			Container.Bind<IShootingService>( ).To<ShootingService>( ).AsSingle( ).NonLazy( );
		}
	}
}