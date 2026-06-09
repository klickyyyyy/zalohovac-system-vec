using JSONCreator.Presentation.Components;
using JSONCreator.Presentation.Windows;

namespace JSONCreator
{
    public class Program
    {
        static void Main(string[] args)
        {
            Application app = new Application();
            IWindow mainWindow = new MainMenuWindow(app);
            mainWindow.Show();
        }
    }
}
