using UnityEngine;
using Zenject;

namespace Scripts.Projectiles
{
	[CreateAssetMenu( fileName = "ProjectilesInstaller", menuName = "Installers/ProjectilesInstaller" )]

	public class ProjectilesInstaller : ScriptableObjectInstaller 
	{
		public ProjectilesConfig config;

		public override void InstallBindings( )
		{
			Container.BindInstance( config );
			Container.Bind<IProjectilesService>( ).To<ProjectilesService>( ).AsSingle( ).NonLazy( );
		}
	}
}