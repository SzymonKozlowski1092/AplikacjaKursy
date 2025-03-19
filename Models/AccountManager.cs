using RestSharp;
using System.Windows;

namespace Dziekanat.Models
{
    public class AccountManager
    {
        private readonly RestClient _restClient;

        public AccountManager()
        {
            _restClient = new RestClient("https://localhost:7203");
        }

        public async Task<bool> Register(RegisterUser newUser, string role)
        {
            if (newUser == null)
                throw new ArgumentNullException(nameof(newUser));

            if (string.IsNullOrWhiteSpace(role))
            {
                MessageBox.Show("Nie podano roli użytkownika", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            string requestString = GetRegistrationEndpoint(role);

            if (string.IsNullOrEmpty(requestString))
            {
                MessageBox.Show("Nieznana rola użytkownika", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            var request = new RestRequest(requestString, Method.Post).AddJsonBody(newUser);

            try
            {
                var response = await _restClient.ExecuteAsync(request);

                if (response.IsSuccessful)
                {
                    MessageBox.Show("Pomyślnie utworzono użytkownika", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                    return true;
                }

                MessageBox.Show($"Błąd podczas tworzenia użytkownika: {response.Content}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas rejestracji: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public async Task<string> Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Email i hasło są wymagane");
            }

            var request = new RestRequest("/Account/Login", Method.Post)
                .AddJsonBody(new
                {
                    Email = email,
                    Password = password
                });

            try
            {
                var response = await _restClient.ExecuteAsync(request);

                if (!response.IsSuccessful)
                {
                    MessageBox.Show("Błędny email lub hasło", "Błąd logowania", MessageBoxButton.OK, MessageBoxImage.Error);
                    return string.Empty;
                }

                return response.Content.Trim('\"', '/');
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas logowania: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return string.Empty;
            }
        }

        private static string GetRegistrationEndpoint(string role)
        {
            return role switch
            {
                string r when r.Contains("Student", StringComparison.OrdinalIgnoreCase) => "Account/Register/Student",
                string r when r.Contains("Lecturer", StringComparison.OrdinalIgnoreCase) => "Account/Register/Lecturer",
                _ => string.Empty
            };
        }
    }
}
