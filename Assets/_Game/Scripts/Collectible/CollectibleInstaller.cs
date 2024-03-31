using UnityEngine;
using Zenject;

namespace Scripts.Collectible
{
	[CreateAssetMenu( fileName = "CollectibleInstaller", menuName = "Installers/CollectibleInstaller" )]

	public class CollectibleInstaller : ScriptableObjectInstaller 
	{
		public CollectibleConfig config;

		public override void InstallBindings( )
		{
			Container.BindInstance( config );
			Container.Bind<ICollectibleService>( ).To<CollectibleService>( ).AsSingle( ).NonLazy( );
		}
	}
}