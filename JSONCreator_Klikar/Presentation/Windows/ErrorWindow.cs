using JSONCreator.Presentation.Components;

namespace JSONCreator.Presentation.Windows
{
    public class ErrorWindow : BaseWindow
    {
        public ErrorWindow(string title, string message, Application application, IWindow? returnWindow = null)
            : base(title, application, returnWindow)
        {
            Label messageLabel = new Label(message);
            messageLabel.Color = ConsoleColor.Red;
            Button okButton = new Button("OK");

            RegisterComponent(messageLabel);
            RegisterComponent(okButton);

            okButton.Clicked += () => Close();
        }
    }
}
