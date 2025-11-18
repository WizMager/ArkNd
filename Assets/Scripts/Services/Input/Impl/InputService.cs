using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Services.Input.Impl
{
    public class InputService : IInputService, IDisposable
    {
        private readonly InputSystem_Actions _inputSystemActions = new();
        
        public Action<bool> OnAttack { get; set; }
        public bool IsMove { get; private set; }
        public float MoveDirection { get; private set; }

        private bool _isAttack;

        public void Initialize()
        {
            _inputSystemActions.Enable();
            _inputSystemActions.Player.Move.performed += OnMovePerformed;
            _inputSystemActions.Player.Move.canceled += OnMoveCanceled;
            
            _inputSystemActions.Player.Attack.performed += OnAttackPerform;
            _inputSystemActions.Player.Attack.canceled += OnStopAttack;
        }

        private void OnMovePerformed(InputAction.CallbackContext context)
        {
            IsMove = true;
            MoveDirection = context.ReadValue<Vector2>().x;
        }
        
        private void OnMoveCanceled(InputAction.CallbackContext obj)
        {
            IsMove = false;
        }

        private void OnAttackPerform(InputAction.CallbackContext obj)
        {
            if (_isAttack) 
                return;
            
            _isAttack = true;
            
            OnAttack?.Invoke(true);
        }
        
        private void OnStopAttack(InputAction.CallbackContext obj)
        {
            if (!_isAttack) 
                return;
            
            _isAttack = false;
            
            OnAttack?.Invoke(false);
        }
        
        public void Dispose()
        {
            _inputSystemActions.Player.Move.performed -= OnMovePerformed;
            _inputSystemActions.Player.Move.canceled -= OnMoveCanceled;
            _inputSystemActions.Player.Attack.performed -= OnAttackPerform;
            _inputSystemActions.Player.Attack.canceled -= OnStopAttack;
            
            _inputSystemActions.Disable();
            _inputSystemActions.Dispose();
        }
    }
}