using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay
{
    /// <summary>
    /// Input handler for paddle movement and click detection.
    /// </summary>
    public class PaddleInput
    {
        private readonly InputSystem_Actions _actions = null;

        public Vector2 PointerPosition { get; private set; }
        public bool Clicked { get; private set; }

        public PaddleInput()
        {
            _actions = new InputSystem_Actions();

            _actions.UI.Point.performed += OnPointerPerformed;
            _actions.UI.Click.performed += OnClickPerformed;

            _actions.Enable();
        }

        public void ResetClick()
        {
            Clicked = false;
        }

        public void Dispose()
        {
            _actions.UI.Point.performed -= OnPointerPerformed;
            _actions.UI.Click.performed -= OnClickPerformed;

            _actions.Disable();
        }
        
        private void OnPointerPerformed(InputAction.CallbackContext context)
        {
            PointerPosition = context.ReadValue<Vector2>();
        }

        private void OnClickPerformed(InputAction.CallbackContext context)
        {
            Clicked = true;
        }
    }
}