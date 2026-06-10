namespace JSONCreator.Presentation.Components
{
    public class Table<T> : IComponent where T : class
    {
        public List<T> Items { get; set; } = new();
        public T? SelectedItem { get; private set; }

        public bool IsInteractive => true;

        public void Render(int? number)
        {
            Console.WriteLine($"{number}. [Tabulka - {Items.Count} položek]");

            if (Items.Count == 0)
            {
                Console.WriteLine("   (žádné záznamy)");
                return;
            }

            for (int i = 0; i < Items.Count; i++)
                Console.WriteLine($"   {i + 1}. {Items[i]}");

            if (SelectedItem != null)
                Console.WriteLine($"   >> Vybrán: {SelectedItem}");
        }

        public void Interact()
        {
            if (Items.Count == 0)
            {
                Console.WriteLine("   Žádné položky.");
                Console.ReadLine();
                return;
            }

            Console.Write($"   Vyberte řádek (1-{Items.Count}, 0 = nic): ");
            string? input = Console.ReadLine();

            if (int.TryParse(input, out int choice))
            {
                if (choice >= 1 && choice <= Items.Count)
                    SelectedItem = Items[choice - 1];
                else if (choice == 0)
                    SelectedItem = null;
            }
        }
    }
}
