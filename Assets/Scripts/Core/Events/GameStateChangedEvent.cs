namespace Core.Events
{
    public readonly struct GameStateChangedEvent
    {
        public GameManager.GameState NewState { get; }

        public GameStateChangedEvent(GameManager.GameState state)
        {
            NewState = state;
        }
    }
}