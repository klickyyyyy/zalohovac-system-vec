using JSONCreator.Presentation.Components;

namespace JSONCreator.Presentation.Windows
{
    public class DialogWindow : BaseWindow
    {
        public DialogWindow(string title, string message, Application application, IWindow? returnWindow = null)
            : base(title, application, returnWindow)
        {
            Label messageLabel = new Label(message);
            Button yesButton = new Button("Ano");
            Button noButton = new Button("Ne");

            RegisterComponent(messageLabel);
            RegisterComponent(yesButton);
            RegisterComponent(noButton);

            yesButton.Clicked += () => Submit();
            noButton.Clicked += () => Close();
        }
    }
}
