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
        private const string StartText = "Начать игру";
        private const string WinText   = "Вы выиграли!";
        private const string LoseText  = "Вы проиграли!";
        private readonly UIMessageView _view = null;
        private readonly GameManager _game = null;
        private readonly EventBus _bus = null;

        public UIMessageController(UIMessageView view, GameManager game, EventBus bus)
        {
            _view = view;
            _game = game;
            _bus = bus;

            _bus.Subscribe<GameStateChangedEvent>(OnGameStateChanged);
            _view.SetButtonListener(OnButtonClicked);

            OnGameStateChanged(new GameStateChangedEvent(_game.State));
        }

        public override void Dispose()
        {
            _bus.Unsubscribe<GameStateChangedEvent>(OnGameStateChanged);
            _view.SetButtonListener(null);
        }

        private void OnGameStateChanged(GameStateChangedEvent evt)
        {
            switch (evt.NewState)
            {
                case GameManager.GameState.ReadyToStart:
                {
                    _view.Show(StartText);
                    break;
                }

                case GameManager.GameState.Playing:
                {
                    _view.Hide();
                    break;
                }

                case GameManager.GameState.Win:
                {
                    _view.Show(WinText);
                    break;
                }

                case GameManager.GameState.Lose:
                {
                    _view.Show(LoseText);
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