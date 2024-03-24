using System;
using Scripts.PlayerLoop;
using TMPro;
using UnityEngine;
using Zenject;

namespace Scripts.Distance
{
    [RequireComponent( typeof(TextMeshProUGUI) )]
    public class BestDistanceTextView : MonoBehaviour
    {
        private TextMeshProUGUI _textMesh;

        [Inject] private IPlayerLoopService _playerLoopService;
        [Inject] private IDistanceService   _distanceService;

        private void Awake( )
        {
            _textMesh = GetComponent<TextMeshProUGUI>( );

            _playerLoopService.OnLoopEnded += UpdateText;
        }

        private void OnDestroy( )
        {
            _playerLoopService.OnLoopEnded -= UpdateText;
        }

        private void UpdateText( )
        {
            if ( Math.Abs( _distanceService.Distance - _distanceService.BestDistance ) > 0 )
                UpdateDistance( );
            else
                UpdateDistanceAsNewBest( );
        }

        private void UpdateDistance( )
        {
            _textMesh.text = $"Distance: {_distanceService.Distance}m\n<size=80%>Best distance: {_distanceService.BestDistance}m";
        }

        private void UpdateDistanceAsNewBest( )
        {
            _textMesh.text = $"Distance: {_distanceService.Distance}m\n<size=80%>New best distance!";
        }
    }
}