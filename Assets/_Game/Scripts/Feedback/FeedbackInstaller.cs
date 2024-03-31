using UnityEngine;
using Zenject;

namespace Scripts.Feedback
{
	[CreateAssetMenu( fileName = "FeedbackInstaller", menuName = "Installers/FeedbackInstaller" )]

	public class FeedbackInstaller : ScriptableObjectInstaller 
	{
		public FeedbackConfig config;

		public override void InstallBindings( )
		{
			Container.BindInstance( config );
			Container.Bind<IFeedbackService>( ).To<FeedbackService>( ).AsSingle( ).NonLazy( );
		}
	}
}