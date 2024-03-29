using UnityEngine;
using Zenject;

namespace Scripts.Enemy
{
	[CreateAssetMenu( fileName = "EnemyInstaller", menuName = "Installers/EnemyInstaller" )]

	public class EnemyInstaller : ScriptableObjectInstaller 
	{
		public EnemyConfig config;

		public override void InstallBindings( )
		{
			Container.BindInstance( config );
			Container.Bind<IEnemyService>( ).To<EnemyService>( ).AsSingle( ).NonLazy( );
		}
	}
}