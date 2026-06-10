namespace JSONCreator.Presentation.Components
{
    public class TextBox : IComponent
    {
        private string _label;
        private int _maxLength;

        public string Value { get; set; } = string.Empty;
        public bool IsInteractive => true;

        public TextBox(string label, int maxLength)
        {
            _label = label;
            _maxLength = maxLength;
        }

        public void Render(int? number)
        {
            Console.WriteLine($"{number}. {_label}[{Value}]");
        }

        public void Interact()
        {
            Console.Write($"   Zadejte hodnotu: ");
            string? input = Console.ReadLine();
            if (input != null)
                Value = input.Length > _maxLength ? input[.._maxLength] : input;
        }
    }
}
