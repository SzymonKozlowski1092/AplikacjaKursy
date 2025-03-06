using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Dziekanat.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Dziekanat.ViewModel
{
    public class RegistrationViewModel : ObservableObject
    {
        private readonly AccountManager _accountManager = new AccountManager();

        private string? _firstName;
        private string? _lastName;
        private string? _email;
        private DateTime? _dateOfBirth;
        private string? _password;
        private string? _confirmPassword;
        private string? _accountType;

        public string? FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }
        public string? LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }
        public string? Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }
        public DateTime? DateOfBirth
        {
            get => _dateOfBirth;
            set => SetProperty(ref _dateOfBirth, value);
        }
        public string? Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }
        public string? ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
        }
        public string? AccountType
        {
            get => _accountType;
            set => SetProperty(ref _accountType, value);
        }

        public ICommand RegisterCommand => new AsyncRelayCommand<object>(async (object passwordBox) =>
        {
            try
            {
                if (passwordBox is PasswordBox passwordControl)
                {
                    Password = passwordControl.Password;
                    ConfirmPassword = passwordControl.Password;
                }

                if (!ValidateInput())
                {
                    MessageBox.Show("Uzupełnij wszystkie wymagane pola.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var newUser = new RegisterUser
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    Email = Email,
                    DateOfBirth = DateOfBirth,
                    Password = Password,
                    ConfirmPassword = ConfirmPassword
                };

                var result = await _accountManager.Register(newUser, AccountType);

                if (result)
                {
                    MessageBox.Show("Konto zostało pomyślnie utworzone.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Nie udało się utworzyć konta.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas rejestracji: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        });

        private bool ValidateInput()
        {
            return !string.IsNullOrWhiteSpace(FirstName) &&
                   !string.IsNullOrWhiteSpace(LastName) &&
                   !string.IsNullOrWhiteSpace(Email) &&
                   DateOfBirth.HasValue &&
                   !string.IsNullOrWhiteSpace(Password) &&
                   !string.IsNullOrWhiteSpace(ConfirmPassword) &&
                   !string.IsNullOrWhiteSpace(AccountType);
        }
    }
}
