using System;
using Scripts.Camera;
using UnityEngine;
using Zenject;

namespace Scripts.Ui
{
    public class CanvasCameraFinder : MonoBehaviour
    {
        [SerializeField] private Canvas canvasCached;
        
        [Inject] private ICameraService _cameraService;

        private void Reset( )
        {
            canvasCached = GetComponent<Canvas>( );
        }

        private void Awake( )
        {
            canvasCached.worldCamera = _cameraService.CameraView.cameraObject;
        }
    }
}
