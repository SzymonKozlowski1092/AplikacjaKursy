using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Dziekanat
{
    public class TokenManagment
    {
        public static string GetRoleFromToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Nieprawidłowy format tokena", nameof(token));

            var handler = new JwtSecurityTokenHandler();

            if (!handler.CanReadToken(token))
                throw new ArgumentException("Nieprawidłowy format tokena", nameof(token));

            try
            {
                var jwtToken = handler.ReadJwtToken(token);
                var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type.Equals("role", StringComparison.OrdinalIgnoreCase));

                return roleClaim?.Value ?? throw new InvalidOperationException("Wystąpił błąd podczas odczytywania danych z tokena");
            }
            catch (Exception ex)
            {
                throw new Exception($"Wystąpił błąd podczas odczytywania roli z tokena: {ex.Message}", ex);
            }
        }

        public static ClaimsPrincipal GetClaimsFromToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Nieprawidłowy format tokena", nameof(token));

            var tokenHandler = new JwtSecurityTokenHandler();

            if (!tokenHandler.CanReadToken(token))
                throw new ArgumentException("Nieprawidłowy format tokena", nameof(token));

            try
            {
                var jwtToken = tokenHandler.ReadJwtToken(token);
                var claimsIdentity = new ClaimsIdentity(jwtToken.Claims, "Jwt");


                return new ClaimsPrincipal(claimsIdentity);
            }
            catch (Exception ex)
            {
                throw new Exception($"Wystąpił błąd podczas odczytywania danych z tokena: {ex.Message}", ex);
            }
        }
    }
}
