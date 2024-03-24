using System;
using Scripts.PlayerLoop;
using TMPro;
using UnityEngine;
using Zenject;

namespace Scripts.Distance
{
	[RequireComponent( typeof(TextMeshProUGUI) )]
	public class DistanceTextView : MonoBehaviour
	{
		private TextMeshProUGUI _textMesh;

		[Inject] private IDistanceService _distanceService;

		private void Awake( )
		{
			_textMesh = GetComponent<TextMeshProUGUI>( );
			
			UpdateDistance( );
			_distanceService.OnDistanceUpdated += UpdateDistance;
		}

		private void OnDestroy( )
		{
			_distanceService.OnDistanceUpdated -= UpdateDistance;
		}

		private void UpdateDistance( )
		{
			_textMesh.text = $"{_distanceService.Distance}m";
		}
	}
}