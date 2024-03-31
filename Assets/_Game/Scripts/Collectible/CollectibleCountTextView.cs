using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

namespace Scripts.Collectible
{
    public class CollectibleCountTextView : MonoBehaviour
    {
        [Inject] private ICollectibleService _collectibleService;

        private TextMeshProUGUI _textMesh;

        private void Awake( )
        {
            _textMesh = GetComponentInChildren<TextMeshProUGUI>( );

            UpdateText( );
            _collectibleService.OnUpdateCount += UpdateText;
        }

        private void OnDestroy( )
        {
            _collectibleService.OnUpdateCount -= UpdateText;
        }

        private void UpdateText( )
        {
            _textMesh.SetText( _collectibleService.CollectedCount.ToString( ) );
        }
    }
}