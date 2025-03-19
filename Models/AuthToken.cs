namespace Dziekanat.Models
{
    public class AuthToken
    {
        private static AuthToken _instance;

        public static AuthToken Instance => _instance ??= new AuthToken();

        public string JwtToken { get; set; }
    }

}
