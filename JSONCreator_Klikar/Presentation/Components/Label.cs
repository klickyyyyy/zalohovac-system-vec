namespace JSONCreator.Presentation.Components
{
    public class Label : IComponent
    {
        public string Text { get; set; }
        public ConsoleColor Color { get; set; } = ConsoleColor.White;

        public bool IsInteractive => false;

        public Label(string text)
        {
            Text = text;
        }

        public void Render(int? number)
        {
            ConsoleColor original = Console.ForegroundColor;
            Console.ForegroundColor = Color;
            Console.WriteLine(Text);
            Console.ForegroundColor = original;
        }

        public void Interact() { }
    }
}
