namespace JSONCreator.Presentation.Components
{
    internal interface IComponent
    {
        bool IsInteractive { get; }
        void Render(int? number);
        void Interact();
    }
}
