using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class GameBootstrap : MonoBehaviour
    {
        [SerializeField]
        private string _gameSceneName = "Game";

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        
            if (SceneManager.GetActiveScene().name == _gameSceneName)
            {
                Debug.LogWarning("[GameBootstrap] Game scene is already loaded.");
                return;
            }

            LoadGameScene();
        }

        private void LoadGameScene()
        {
            SceneManager.LoadSceneAsync(_gameSceneName, LoadSceneMode.Single);
        }
    }
}
