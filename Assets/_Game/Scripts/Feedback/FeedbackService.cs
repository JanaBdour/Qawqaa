using UnityEngine;
using Zenject;

namespace Scripts.Feedback
{
	public class FeedbackService : IFeedbackService
	{
		private FeedbackConfig _config;

		[Inject]
		private void Construct( FeedbackConfig config )
		{
			_config = config;
		}
	}
}