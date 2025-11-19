using UnityEngine;
using Core;
using Core.Events;

namespace Gameplay.Bricks
{
    /// <summary>
    /// Abstract base class for all Brick types. Handles collision and destruction events.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public abstract class BrickBase : MonoBehaviour
    {
        private const string BallTag = "Ball";

        private EventBus _eventBus = null;

        protected virtual void Awake()
        {
            _eventBus = ServiceLocator.Resolve<EventBus>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            bool isBall = collision.collider.CompareTag(BallTag);

            if (isBall)
            {
                Hit();
            }
        }
        
        protected virtual void Hit()
        {
            _eventBus.Publish(new BrickDestroyedEvent(this));

            Destroy(gameObject, 0.05f);
        }
    }
}