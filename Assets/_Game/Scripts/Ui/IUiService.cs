using UnityEngine;

namespace Scripts.Ui
{
    public interface IUiService
    {
        void OpenWindow( string name );
        void CloseWindow( string name );
        void Register( UiView   ui );
    }
}
