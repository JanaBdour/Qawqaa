using UnityEngine;
using Zenject;

namespace Scripts.Combo
{
	[CreateAssetMenu( fileName = "ComboInstaller", menuName = "Installers/ComboInstaller" )]

	public class ComboInstaller : ScriptableObjectInstaller 
	{
		public ComboConfig config;

		public override void InstallBindings( )
		{
			Container.BindInstance( config );
			Container.Bind<IComboService>( ).To<ComboService>( ).AsSingle( ).NonLazy( );
		}
	}
}