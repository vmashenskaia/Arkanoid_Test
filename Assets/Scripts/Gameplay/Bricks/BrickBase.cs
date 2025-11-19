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
        private EventBus _eventBus = null;
        private bool _destroyed = false;

        protected virtual void Awake()
        {
            _eventBus = ServiceLocator.Resolve<EventBus>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            bool isBall = collision.collider.CompareTag(Tags.Ball);

            if (isBall)
            {
                Hit();
            }
        }
        
        protected virtual void Hit()
        {
            _eventBus.Publish(new BrickDestroyedEvent(this));

            if (_destroyed) return;
            _destroyed = true;
            Destroy(gameObject);
        }
    }
}