using Scripts.Ticking;
using UnityEngine;
using Zenject;

namespace Scripts.Ui
{
    public class UiService : IUiService
    {
        private UiConfig _config;
        private UiView   _ui;

        [Inject]
        private void Construct( UiConfig config, ITickingService tickingService )
        {
            _config = config;
            
            tickingService.OnStarted += OnStarted;
        }

        private void OnStarted( )
        {
            //ShowWindow( "MainWindow" );
        }

        public void OpenWindow( string name )
        {
            if ( !_ui ) return;

            foreach ( var window in _ui.windows )
            {
                if ( window.name == name )
                    window.Open( );
                else
                    window.Close( );
            }
        }

        public void CloseWindow( string name )
        {
            if ( !_ui ) return;
     
            foreach ( var window in _ui.windows )
            {
                if ( window.name == name )
                    window.Close( );
            }
        }

        public void Register( UiView ui )
        {
            _ui = ui;
        }
    }
}
