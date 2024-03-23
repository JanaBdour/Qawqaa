using UnityEngine;
using Zenject;

namespace Scripts.Player
{
	[CreateAssetMenu( fileName = "PlayerInstaller", menuName = "Installers/PlayerInstaller" )]

	public class PlayerInstaller : ScriptableObjectInstaller 
	{
		public PlayerConfig config;

		public override void InstallBindings( )
		{
			Container.BindInstance( config );
			Container.Bind<IPlayerService>( ).To<PlayerService>( ).AsSingle( ).NonLazy( );
		}
	}
}