using System;

namespace Services.Input
{
    public interface IInputService : IDisposable
    {
        Action<bool> OnAttack { get; set; }
        bool IsMove { get; }
        float MoveDirection { get; }
        
        void Initialize();
    }
}