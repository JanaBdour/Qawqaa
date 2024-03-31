using UnityEngine;
using Zenject;

namespace Scripts.Collectible
{
	public class CollectibleService : ICollectibleService
	{
		private CollectibleConfig _config;

		[Inject]
		private void Construct( CollectibleConfig config )
		{
			_config = config;
		}
	}
}