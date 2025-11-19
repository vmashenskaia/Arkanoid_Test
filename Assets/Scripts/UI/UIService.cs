using System;
using System.Collections.Generic;
using Core;
using Core.Events;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// Provides creation, caching and management of UI Views and their controllers.
    /// </summary>
    public class UIService
    {
        private readonly UIViewsConfig _config = null;
        private Canvas _canvas = null;
        private readonly Dictionary<Type, BaseView> _views = new Dictionary<Type, BaseView>();
        private readonly Dictionary<Type, BaseUIController> _controllers = new Dictionary<Type, BaseUIController>();
        
        public UIService(UIViewsConfig config)
        {
            _config = config;
        }

        public T GetView<T>() where T : BaseView
        {
            Type type = typeof(T);

            if (_canvas == null)
            {
                _canvas = CreateCanvas();
            }

            if (_views.TryGetValue(type, out BaseView cachedView))
            {
                return (T)cachedView;
            }

            for (int i = 0; i < _config.Views.Count; i++)
            {
                BaseView prefab = _config.Views[i];

                if (prefab is T typedPrefab)
                {
                    T instance = UnityEngine.Object.Instantiate(typedPrefab, _canvas.transform);
                    instance.Initialize();

                    if (typedPrefab.Cacheable)
                    {
                        _views[type] = instance;
                    }

                    AttachController(type, instance);

                    return instance;
                }
            }

            throw new Exception("UIService: View " + type.Name + " not found in config.");
        }

        public void Dispose()
        {
            foreach (KeyValuePair<Type, BaseUIController> kvp in _controllers)
            {
                kvp.Value.Dispose();
            }

            _controllers.Clear();
            _views.Clear();
        }

        private Canvas CreateCanvas()
        {
            GameObject go = new GameObject("MainCanvas");
            UnityEngine.Object.DontDestroyOnLoad(go);

            Canvas canvas = go.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            go.AddComponent<CanvasScaler>();
            go.AddComponent<GraphicRaycaster>();

            return canvas;
        }

        private void AttachController(Type viewType, BaseView view)
        {
            if (_controllers.ContainsKey(viewType))
            {
                return;
            }

            GameManager gm = GameManager.Instance;
            if (gm == null)
            {
                Debug.LogError("UIService: GameManager.Instance is null while creating controller.");
                return;
            }

            EventBus bus = ServiceLocator.Resolve<EventBus>();
            BaseUIController controller = null;

            if (view is UIMessageView msgView)
            {
                controller = new UIMessageController(msgView, gm, bus);
            }

            if (controller != null)
            {
                _controllers[viewType] = controller;
            }
        }
    }
}