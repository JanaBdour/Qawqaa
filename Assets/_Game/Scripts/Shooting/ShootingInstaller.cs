using UnityEngine;
using Zenject;

namespace Scripts.Shooting
{
	[CreateAssetMenu( fileName = "ShootingInstaller", menuName = "Installers/ShootingInstaller" )]

	public class ShootingInstaller : ScriptableObjectInstaller 
	{
		public ShootingConfig configNoAim;
		public ShootingConfig configWithAim;

		public override void InstallBindings( )
		{
			Container.BindInstance( StaticToggleTapToMove.TapToMove ? configNoAim : configWithAim );
			if ( !StaticToggleTapToMove.TapToMove )
				Container.Bind<IShootingService>( ).To<ShootingService>( ).AsSingle( ).NonLazy( );
			else
				Container.Bind<IShootingService>( ).To<NoAimShootingService>( ).AsSingle( ).NonLazy( );
		}
	}
}