using UnityEngine;

namespace UI
{
    /// <summary>
    /// Base class for all UI views.
    /// Provides visibility control and optional caching behavior.
    /// </summary>
    public abstract class BaseView : MonoBehaviour
    {
        [Header("View Settings")]
        [SerializeField] 
        private GameObject _root = null;

        [SerializeField] 
        private bool _cacheable = true;

        public bool Cacheable
        {
            get { return _cacheable; }
        }

        public bool IsVisible
        {
            get { return _root.activeSelf; }
        }

        public virtual void Show()
        {
            _root.SetActive(true);
        }

        public virtual void Hide()
        {
            _root.SetActive(false);
        }

        public virtual void Initialize()
        {
        }
    }
}