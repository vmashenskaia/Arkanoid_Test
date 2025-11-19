namespace Gameplay.Bricks
{
    /// <summary>
    /// Event fired when a Brick has been destroyed.
    /// </summary>
    public readonly struct BrickDestroyedEvent
    {
        public readonly BrickBase brick;

        public BrickDestroyedEvent(BrickBase brick)
        {
            this.brick = brick;
        }
    }
}