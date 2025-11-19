using System;
using System.Collections.Generic;
using Core.Events;
using Gameplay.Bricks;
using Gameplay.Level;
using UI;
using UnityEngine;

namespace Core
{
    /// <summary>
    /// Manages game state, Brick types, level initialization and global events routing.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [Header("Bricks Types")]
        [SerializeField]
        private List<BrickBase> _brickPrefabs = null;

        [Header("UI")]
        [SerializeField]
        private UIViewsConfig _uiViewsConfig = null;

        private LevelManager _levelManager = null;
        private EventBus _bus = null;

        public static GameManager Instance { get; private set; } = null;

        public List<BrickBase> BrickPrefabs
        {
            get { return _brickPrefabs; }
        }

        public UIViewsConfig UIViewsConfig
        {
            get { return _uiViewsConfig; }
        }

        public enum GameState
        {
            ReadyToStart,
            Playing,
            Win,
            Lose,
        }

        public GameState State { get; private set; } = GameState.ReadyToStart;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            _bus = ServiceLocator.Resolve<EventBus>();
        }

        private void Start()
        {
            UIService ui = ServiceLocator.Resolve<UIService>();
            ui.GetView<UIMessageView>();

            _levelManager = ServiceLocator.Resolve<LevelManager>();
            _levelManager.InitializeLevel();

            _bus.Publish(new GameStateChangedEvent(State));
        }

        private void OnDestroy()
        {
            _levelManager.Dispose();
        }

        public void StartGame()
        {
            SetState(GameState.Playing);
            _bus.Publish(new GameStartedEvent());
        }

        public void SetWin()
        {
            if (State != GameState.Playing)
            {
                return;
            }

            SetState(GameState.Win);
        }

        public void SetLose()
        {
            if (State != GameState.Playing)
            {
                return;
            }

            SetState(GameState.Lose);
        }

        public void RestartLevel()
        {
            _bus.Publish(new GameRestartEvent());
            SetState(GameState.Playing);
        }
        
        private void SetState(GameState newState)
        {
            State = newState;
            _bus.Publish(new GameStateChangedEvent(newState));
        }
    }
}