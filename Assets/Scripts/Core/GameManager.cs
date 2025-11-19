using UnityEngine;

namespace Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public enum GameState
        {
            Playing,
            Win,
            Lose
        }

        public GameState State { get; private set; } = GameState.Playing;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void SetWin()
        {
            if (State != GameState.Playing)
                return;

            State = GameState.Win;
            Debug.Log("[GameManager] WIN");
        }

        public void SetLose()
        {
        }

        public void RestartLevel()
        {
        }
    }
}