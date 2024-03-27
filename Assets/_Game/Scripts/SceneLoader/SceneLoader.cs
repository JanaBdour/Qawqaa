using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.SceneLoader
{
    public class SceneLoader : MonoBehaviour
    {
        public string coreSceneName = "Core";

        public List<string> additionalScenesToLoad;

        private void Awake()
        {
            SceneManager.sceneLoaded += HandleSceneLoaded;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= HandleSceneLoaded;
        }

        private void Start()
        {
        }
        
        public void HandleLoadScenes()
        {
            SceneManager.LoadSceneAsync( coreSceneName, LoadSceneMode.Additive );
        }

        private void HandleSceneLoaded( Scene loadedScene, LoadSceneMode loadMode )
        {
            if ( loadedScene.name != coreSceneName ) 
                return;
            
            foreach ( var additionalScene in additionalScenesToLoad )
            {
                SceneManager.LoadSceneAsync( additionalScene, LoadSceneMode.Additive );
            }

            SceneManager.UnloadSceneAsync( 0 );
        }
    }
}
