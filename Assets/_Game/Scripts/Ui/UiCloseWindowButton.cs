using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Scripts.Ui
{
    public class UiCloseWindowButton : MonoBehaviour
    {
        [SerializeField] private string windowName;

        [Inject] private IUiService _uiService;
        private          Button     _button;

        private void Awake( )
        {
            _button = GetComponent<Button>( );
            _button.onClick.AddListener( Close );
        }

        private void OnDestroy( )
        {
            _button.onClick.RemoveListener( Close );
        }

        private void Close( )
        {
            _uiService.CloseWindow( windowName );
        }
    }
}