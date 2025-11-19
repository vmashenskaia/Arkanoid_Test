using System.Collections.Generic;
using Core;
using Core.Events;
using Gameplay.Bricks;
using UnityEngine;

namespace Gameplay.Level
{
    /// <summary>
    /// Manages brick generation, destruction tracking and level reset.
    /// </summary>
    public class LevelManager
    {
        private readonly EventBus _eventBus;

        private List<BrickBase> _bricks = null;
        private Transform _brickRoot = null;

        public LevelManager(EventBus eventBus)
        {
            _eventBus = eventBus;

            _eventBus.Subscribe<GameRestartEvent>(OnRestart);
            _eventBus.Subscribe<BrickDestroyedEvent>(OnBrickDestroyed);
        }

        public void InitializeLevel()
        {
            ClearOldLevel();

            _brickRoot = new GameObject("Bricks").transform;

            LevelGenerator generator = new LevelGenerator(GameManager.Instance.BrickPrefabs);
            _bricks = generator.GenerateLevel(_brickRoot);

            Debug.Log("[LevelManager] Generated " + _bricks.Count + " bricks.");
        }

        public void Dispose()
        {
            _eventBus.Unsubscribe<GameRestartEvent>(OnRestart);
            _eventBus.Unsubscribe<BrickDestroyedEvent>(OnBrickDestroyed);
        }

        private void ClearOldLevel()
        {
            if (_brickRoot != null)
            {
                Object.Destroy(_brickRoot.gameObject);
            }

            _bricks = new List<BrickBase>();
        }

        private void OnRestart(GameRestartEvent _)
        {
            InitializeLevel();
        }

        private void OnBrickDestroyed(BrickDestroyedEvent evt)
        {
            _bricks.Remove(evt.brick);

            if (_bricks.Count == 0)
            {
                GameManager.Instance.SetWin();
            }
        }
    }
}