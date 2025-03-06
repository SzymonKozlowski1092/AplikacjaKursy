using CommunityToolkit.Mvvm.Input;
using Dziekanat.Views;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Dziekanat.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Dziekanat.ViewModel
{
    public class LoginViewModel : ObservableObject
    {
        private readonly AccountManager _accountManager;

        private string? _email;
        private string? _password;
        private string _role;
        private string _userId;

        public string Role
        {
            get => _role;
            set => SetProperty(ref _role, value);
        }
        public string UserId
        {
            get => _userId;
            set => SetProperty(ref _userId, value);
        }
        public string? Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }
        public string? Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public LoginViewModel()
        {
            _accountManager = new AccountManager();
        }

        public ICommand LoginCommand => new AsyncRelayCommand<object>(async (object passwordBox) =>
        {
            try
            {
                if (passwordBox is PasswordBox passwordControl)
                {
                    Password = passwordControl.Password;
                }

                if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
                {
                    MessageBox.Show("Email i hasło są wymagane.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var token = await _accountManager.Login(Email, Password);
                var claimsPrincipal = TokenManagment.GetClaimsFromToken(token);

                UserId = claimsPrincipal.FindFirst("nameid")?.Value ?? string.Empty;
                Role = claimsPrincipal.FindFirst("role")?.Value ?? string.Empty;

                AuthToken.Instance.JwtToken = token;
                OpenRoleSpecificView(Role);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas logowania: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        });

        public ICommand RegisterCommand => new RelayCommand(() =>
        {
            try
            {
                new RegistrationView().ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas otwierania widoku rejestracji: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        });

        private void OpenRoleSpecificView(string role)
        {
            try
            {
                switch (role)
                {
                    case "Student":
                        new StudentView(UserId).ShowDialog();
                        break;

                    case "Lecturer":
                        new LecturerView(UserId).ShowDialog();
                        break;

                    default:
                        MessageBox.Show("Niepoprawne dane logowania.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas otwierania widoku: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
