using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// Displays UI message text and provides a button for user interaction.
    /// </summary>
    public class UIMessageView : BaseView
    {
        [SerializeField] private TMP_Text _message = null;
        [SerializeField] private Button _button = null;
        
        public Button Button
        {
            get { return _button; }
        }

        public void Show(string text)
        {
            _message.text = text;

            base.Show();
        }
    }
}