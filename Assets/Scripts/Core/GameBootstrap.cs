using Core.Events;
using Gameplay;
using UnityEngine;
using UnityEngine.SceneManagement;
using Gameplay.Level;
using UI;

namespace Core
{
    /// <summary>
    /// Entry point responsible for initializing core services and loading the main game scene.
    /// </summary>
    public class GameBootstrap : MonoBehaviour
    {
        [SerializeField]
        private string _gameSceneName = "Game";

        [SerializeField]
        private GameManager _gameManagerPrefab = null;

        private GameManager _gameManager = null;
        private PaddleInput _paddleInput = null;
        private EventBus _eventBus = null;
        private LevelManager _levelManager = null;
        private UIService _uiService = null;

        private bool _servicesInitialized = false;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            InitializeServices();

            if (SceneManager.GetActiveScene().name != _gameSceneName)
            {
                LoadGameScene();
            }
        }

        private void OnDestroy()
        {
            if (_paddleInput != null)
            {
                _paddleInput.Dispose();
            }
            
            if (_uiService != null)
            {
                _uiService.Dispose();
            }
        }

        private void InitializeServices()
        {
            if (_servicesInitialized)
            {
                return;
            }

            _servicesInitialized = true;

            _eventBus = new EventBus();
            ServiceLocator.Register(_eventBus);

            _uiService = new UIService(_gameManagerPrefab.UIViewsConfig);
            ServiceLocator.Register(_uiService);

            _gameManager = Instantiate(_gameManagerPrefab);
            DontDestroyOnLoad(_gameManager);

            _paddleInput = new PaddleInput();
            ServiceLocator.Register(_paddleInput);

            _levelManager = new LevelManager(_eventBus);
            ServiceLocator.Register(_levelManager);

            Debug.Log("[GameBootstrap] Services initialized ONCE.");
        }

        private void LoadGameScene()
        {
            SceneManager.LoadSceneAsync(_gameSceneName, LoadSceneMode.Single);
        }
    }
}