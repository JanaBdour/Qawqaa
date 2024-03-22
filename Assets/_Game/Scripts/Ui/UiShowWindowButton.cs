using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Scripts.Ui
{
    public class UiShowWindowButton : MonoBehaviour
    {
        [SerializeField] private string windowName;

        [Inject] private IUiService _uiService;
        private          Button     _button;

        private void Awake( )
        {
            _button = GetComponent<Button>( );
            _button.onClick.AddListener( Open );
        }

        private void OnDestroy( )
        {
            _button.onClick.RemoveListener( Open );
        }

        private void Open( )
        {
            _uiService.OpenWindow( windowName );
        }
    }
}
