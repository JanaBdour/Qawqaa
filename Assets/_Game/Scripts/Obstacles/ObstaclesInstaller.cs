using UnityEngine;
using Zenject;

namespace Scripts.Obstacles
{
	[CreateAssetMenu( fileName = "ObstaclesInstaller", menuName = "Installers/ObstaclesInstaller" )]

	public class ObstaclesInstaller : ScriptableObjectInstaller 
	{
		public ObstaclesConfig config;

		public override void InstallBindings( )
		{
			Container.BindInstance( config );
			Container.Bind<IObstaclesService>( ).To<ObstaclesService>( ).AsSingle( ).NonLazy( );
		}
	}
}