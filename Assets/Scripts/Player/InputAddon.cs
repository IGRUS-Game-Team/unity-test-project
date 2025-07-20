using UnityEngine;
using UnityEngine.InputSystem;

namespace StarterAssets
{
    public class InputAddon : MonoBehaviour
    {
        [Header("Input Actions")]
        public InputActionReference holdAction;
        public InputActionReference dropAction;
        public InputActionReference setAction;

        private StarterAssetsInputs _inputs;

        private void Awake()
        {
            _inputs = GetComponent<StarterAssetsInputs>();
        }

        private void OnEnable()
        {
            if (holdAction != null)
            {
                holdAction.action.Enable();
                holdAction.action.performed += OnHold;
                holdAction.action.canceled += OnHoldCanceled;
            }

            if (dropAction != null)
            {
                dropAction.action.Enable();
                dropAction.action.performed += OnDrop;
            }

            if (setAction != null)
            {
                setAction.action.Enable();
                setAction.action.performed += OnSet;
                setAction.action.canceled += OnSetCanceled;
            }
        }

        private void OnDisable()
        {
            if (holdAction != null)
            {
                holdAction.action.performed -= OnHold;
                holdAction.action.canceled -= OnHoldCanceled;
            }

            if (dropAction != null)
            {
                dropAction.action.performed -= OnDrop;
            }

            if (setAction != null)
            {
                setAction.action.performed -= OnSet;
                setAction.action.canceled -= OnSetCanceled;
            }
        }

        private void OnHold(InputAction.CallbackContext ctx) => _inputs.HoldInput(true);
        private void OnHoldCanceled(InputAction.CallbackContext ctx) => _inputs.HoldInput(false);
        private void OnDrop(InputAction.CallbackContext ctx) => _inputs.DropInput(true);
        private void OnSet(InputAction.CallbackContext ctx) => _inputs.SetInput(true);
        private void OnSetCanceled(InputAction.CallbackContext ctx) => _inputs.SetInput(false);
    }
}
