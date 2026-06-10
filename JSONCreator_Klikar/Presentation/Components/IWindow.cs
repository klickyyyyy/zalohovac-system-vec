namespace JSONCreator.Presentation.Components
{
    public interface IWindow
    {
        event Action? Submitted;
        void Show();
    }
}
