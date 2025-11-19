using Core;
using Core.Events;

namespace UI
{
    /// <summary>
    /// Handles logic and interactions for the UIMessageView.
    /// Displays state messages and reacts to user input.
    /// </summary>
    public class UIMessageController : BaseUIController
    {
        private readonly UIMessageView _view = null;
        private readonly GameManager _game = null;
        private readonly EventBus _bus = null;

        public UIMessageController(UIMessageView view, GameManager game, EventBus bus)
        {
            _view = view;
            _game = game;
            _bus = bus;

            _bus.Subscribe<GameStateChangedEvent>(OnGameStateChanged);
            _view.Button.onClick.AddListener(OnButtonClicked);

            OnGameStateChanged(new GameStateChangedEvent(_game.State));
        }

        public override void Dispose()
        {
            _bus.Unsubscribe<GameStateChangedEvent>(OnGameStateChanged);
            _view.Button.onClick.RemoveListener(OnButtonClicked);
        }

        private void OnGameStateChanged(GameStateChangedEvent evt)
        {
            switch (evt.NewState)
            {
                case GameManager.GameState.ReadyToStart:
                {
                    _view.Show("Начать игру");
                    break;
                }

                case GameManager.GameState.Playing:
                {
                    _view.Hide();
                    break;
                }

                case GameManager.GameState.Win:
                {
                    _view.Show("Вы выиграли!");
                    break;
                }

                case GameManager.GameState.Lose:
                {
                    _view.Show("Вы проиграли!");
                    break;
                }
            }
        }

        private void OnButtonClicked()
        {
            switch (_game.State)
            {
                case GameManager.GameState.ReadyToStart:
                {
                    _game.StartGame();
                    break;
                }

                case GameManager.GameState.Win:
                case GameManager.GameState.Lose:
                {
                    _game.RestartLevel();
                    break;
                }
            }
        }
    }
}