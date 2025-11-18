using System;

namespace Services.Input
{
    public interface IInputService
    {
        Action<bool> OnAttack { get; set; }
        bool IsMove { get; }
        float MoveDirection { get; }
        
        void Initialize();
    }
}