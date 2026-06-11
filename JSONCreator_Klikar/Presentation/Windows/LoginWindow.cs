using JSONCreator.Helpers;
using JSONCreator.Presentation.Components;

namespace JSONCreator.Presentation.Windows
{
    public class LoginWindow : BaseWindow
    {
        private TextBox _usernameBox;
        private TextBox _passwordBox;
        private Button _loginButton;
        private Button _backButton;

        public LoginWindow(Application application, IWindow? returnWindow = null)
            : base("Přihlášení", application, returnWindow)
        {
            _usernameBox = new TextBox("Uživatelské jméno: \t", 32);
            _passwordBox = new TextBox("Heslo: \t\t\t", 32);
            _loginButton = new Button("Přihlásit se");
            _backButton = new Button("Zpět");

            RegisterComponent(_usernameBox);
            RegisterComponent(_passwordBox);
            RegisterComponent(_loginButton);
            RegisterComponent(_backButton);

            _loginButton.Clicked += LoginButtonClicked;
            _backButton.Clicked += () => Close();
        }

        private void LoginButtonClicked()
        {
            string? token = ApiHelper.Login(_usernameBox.Value, _passwordBox.Value);

            if (token == null)
            {
                IWindow errorWindow = new ErrorWindow("Chyba", "Špatné jméno nebo heslo.", _application, this);
                errorWindow.Show();
                return;
            }

            ApiHelper.AuthToken = token;
            Submit();
        }
    }
}
