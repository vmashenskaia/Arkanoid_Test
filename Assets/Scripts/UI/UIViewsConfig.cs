using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// Holds a list of UI View prefabs used by UIService.
    /// </summary>
    [CreateAssetMenu(fileName = "UIViewsConfig", menuName = "Configs/UI Views")]
    public class UIViewsConfig : ScriptableObject
    {
        public List<BaseView> Views
        {
            get { return _views; }
        }

        [SerializeField] 
        private List<BaseView> _views = null;
    }
}