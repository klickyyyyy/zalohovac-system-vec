using JSONCreator.Entities;
using JSONCreator.Helpers;
using JSONCreator.Presentation.Components;

namespace JSONCreator.Presentation.Windows
{
    public class MainMenuWindow : BaseWindow
    {
        private Button _manageJobsButton;
        private Button _loginButton;
        private Button _quitButton;

        private Label _loginStatus;

        public MainMenuWindow(Application application)
            : base("Json Creator - Zálohovací systém", application)
        {
            _loginStatus = new Label("Nepřihlášen");
            _manageJobsButton = new Button("Spravovat úlohy na serveru");
            _loginButton = new Button("Přihlásit se");
            _quitButton = new Button("Quit");

            RegisterComponent(_loginStatus);
            RegisterComponent(_loginButton);
            RegisterComponent(_manageJobsButton);
            RegisterComponent(_quitButton);

            _loginButton.Clicked += LoginButtonClicked;
            _manageJobsButton.Clicked += ManageJobsButtonClicked;
            _quitButton.Clicked += QuitButtonClicked;
        }

        private void LoginButtonClicked()
        {
            IWindow window = new LoginWindow(_application, this);
            window.Submitted += () =>
            {
                _loginStatus.Text = $"Přihlášen jako: {ApiHelper.AuthToken?[..10]}...";
                _loginStatus.Color = ConsoleColor.Green;
            };
            window.Show();
        }

        private void ManageJobsButtonClicked()
        {
            if (ApiHelper.AuthToken == null)
            {
                IWindow errorWindow = new ErrorWindow("Chyba", "Nejdříve se přihlaste.", _application, this);
                errorWindow.Show();
                return;
            }

            List<BackupJob> jobs = ApiHelper.GetAllJobs();

            IWindow window = new BackupJobListWindow("Správa zálohovacích úloh", jobs, _application, this);
            window.Show();
        }

        private void QuitButtonClicked()
        {
            _application.Stop();
        }
    }
}
