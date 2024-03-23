using UnityEngine;
using Zenject;

namespace Scripts.Player
{
	public class PlayerService : IPlayerService
	{
		public PlayerView Player { get; private set; }
	
		private PlayerConfig _config;

		[Inject]
		private void Construct( PlayerConfig config )
		{
			_config = config;
			Player  = Object.Instantiate( _config.prefab );
		}
	}
}