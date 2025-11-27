using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OSP.SPASv2.Service 
{
    public class JWTAuthenticationManager
    {
        private readonly string key;
        //private readonly IConfiguration _configuration;
        private readonly IDictionary<string, string> users = new Dictionary<string, string>()
        { { "wa","123412"} };

        //public JWTAuthenticationManager(IConfiguration _configuration)
        public JWTAuthenticationManager(string key)
        {
            this.key = key;
        }



        public string Authenticate(string username,string password)
        {
            if (!users.Any(u=> u.Key == username && u.Value == password))
            {
                return null;
            }
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            // var tokenKey = Encoding.ASCII.GetBytes(_configuration["Jwt:Token"]);
            var tokenKey = Encoding.ASCII.GetBytes(key);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, username)
                }),
                // Duration of the Tokenyth
                // Now the the Duration to 1 Hour
                Expires = DateTime.UtcNow.AddHours(1),
                //Expires = DateTime.UtcNow.AddMinutes(2),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature) //setting sha256 algorithm
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

    }
}
