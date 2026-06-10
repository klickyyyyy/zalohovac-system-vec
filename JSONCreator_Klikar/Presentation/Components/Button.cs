namespace JSONCreator.Presentation.Components
{
    public class Button : IComponent
    {
        private string _text;

        public event Action? Clicked;
        public bool IsInteractive => true;

        public Button(string text)
        {
            _text = text;
        }

        public void Render(int? number)
        {
            Console.WriteLine($"{number}. [{_text}]");
        }

        public void Interact()
        {
            Clicked?.Invoke();
        }
    }
}
