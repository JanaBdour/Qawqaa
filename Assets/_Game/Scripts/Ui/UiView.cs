using System;
using UnityEngine;
using Zenject;

namespace Scripts.Ui
{
    public class UiView : MonoBehaviour
    {
        public Window[] windows;

        [Inject] private IUiService _uiService;

        private void Awake( )
        {
            _uiService.Register( this );
        }
    }
}