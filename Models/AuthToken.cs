using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Dziekanat.Models
{
    public class AuthToken
    {
        private static AuthToken _instance;

        public static AuthToken Instance => _instance ??= new AuthToken();

        public string JwtToken { get; set; }
    }

}
