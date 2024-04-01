using UnityEngine;
using Zenject;

namespace Scripts.FallingSteps
{
	[CreateAssetMenu( fileName = "FallingStepsInstaller", menuName = "Installers/FallingStepsInstaller" )]

	public class FallingStepsInstaller : ScriptableObjectInstaller 
	{
		public FallingStepsConfig config;

		public override void InstallBindings( )
		{
			Container.BindInstance( config );
			Container.Bind<IFallingStepsService>( ).To<FallingStepsService>( ).AsSingle( ).NonLazy( );
		}
	}
}