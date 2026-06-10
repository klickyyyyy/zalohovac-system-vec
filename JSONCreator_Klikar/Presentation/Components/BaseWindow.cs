namespace JSONCreator.Presentation.Components
{
    public abstract class BaseWindow : IWindow
    {
        private string _title;
        protected Application _application;
        private List<IComponent> _components = new();
        private bool _done = false;

        public event Action? Submitted;

        protected BaseWindow(string title, Application application, IWindow? returnWindow = null)
        {
            _title = title;
            _application = application;
        }

        private protected void RegisterComponent(IComponent component)
        {
            _components.Add(component);
        }

        protected void Submit()
        {
            _done = true;
            Submitted?.Invoke();
        }

        protected void Close()
        {
            _done = true;
        }

        public void Show()
        {
            _done = false;

            while (!_done && _application.Running)
            {
                Render();
                HandleInput();
            }
        }

        private void Render()
        {
            Console.Clear();
            Console.WriteLine($"=== {_title} ===");
            Console.WriteLine();

            int counter = 0;
            foreach (IComponent component in _components)
            {
                int? number = component.IsInteractive ? ++counter : (int?)null;
                component.Render(number);
            }
        }

        private void HandleInput()
        {
            List<IComponent> interactive = _components.Where(c => c.IsInteractive).ToList();
            if (interactive.Count == 0)
                return;

            Console.WriteLine();
            Console.Write($"Vyberte akci (1-{interactive.Count}): ");
            string? input = Console.ReadLine();

            if (int.TryParse(input, out int choice) && choice >= 1 && choice <= interactive.Count)
                interactive[choice - 1].Interact();
        }
    }
}
