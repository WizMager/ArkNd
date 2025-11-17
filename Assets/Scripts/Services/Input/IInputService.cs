namespace Services.Input
{
    public interface IInputService
    {
        bool IsMove { get; }
        float MoveDirection { get; }
        
        void Initialize();
    }
}