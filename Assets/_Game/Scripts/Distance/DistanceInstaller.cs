using UnityEngine;
using Zenject;

namespace Scripts.Distance
{
	[CreateAssetMenu( fileName = "DistanceInstaller", menuName = "Installers/DistanceInstaller" )]

	public class DistanceInstaller : ScriptableObjectInstaller 
	{
		public DistanceConfig config;

		public override void InstallBindings( )
		{
			Container.BindInstance( config );
			Container.Bind<IDistanceService>( ).To<DistanceService>( ).AsSingle( ).NonLazy( );
		}
	}
}