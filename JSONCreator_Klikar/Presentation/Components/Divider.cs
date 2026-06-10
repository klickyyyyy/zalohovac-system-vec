namespace JSONCreator.Presentation.Components
{
    public class Divider : IComponent
    {
        private string _text;

        public bool IsInteractive => false;

        public Divider(string text)
        {
            _text = text;
        }

        public void Render(int? number)
        {
            if (string.IsNullOrEmpty(_text))
                Console.WriteLine(new string('-', 40));
            else
                Console.WriteLine($"--- {_text} ---");
        }

        public void Interact() { }
    }
}
