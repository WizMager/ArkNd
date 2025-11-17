using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Services.Input.Impl
{
    public class InputService : IInputService, IDisposable
    {
        private readonly InputSystem_Actions _inputSystemActions = new();

        public bool IsMove { get; private set; }
        public float MoveDirection { get; private set; }

        public void Initialize()
        {
            _inputSystemActions.Enable();
            _inputSystemActions.Player.Move.performed += OnMovePerformed;
            _inputSystemActions.Player.Move.canceled += OnMoveCanceled;
        }
        
        private void OnMovePerformed(InputAction.CallbackContext context)
        {
            if (!IsMove)
            {
                IsMove = true;
            }
            
            MoveDirection = context.ReadValue<Vector2>().x;
        }
        
        private void OnMoveCanceled(InputAction.CallbackContext obj)
        {
            if (IsMove)
            {
                IsMove = false;
            }
        }

        public void Dispose()
        {
            _inputSystemActions.Player.Move.performed -= OnMovePerformed;
            _inputSystemActions.Player.Move.canceled -= OnMoveCanceled;
            
            _inputSystemActions.Disable();
            _inputSystemActions.Dispose();
        }
    }
}