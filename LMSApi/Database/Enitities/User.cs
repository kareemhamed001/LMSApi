using LMSApi.App.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LMSApi.Database.Enitities
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        private string _password;
        public string Password
        {
            get
            { return _password; }
            set
            {
                var passwordHasher = new PasswordHasher<object>();
                _password = passwordHasher.HashPassword(null, value);
            }
        }
        public List<Role> Roles { get; set; } = new List<Role>();
        public Support? Support { get; set; }
        public Teacher? Teacher { get; set; }
        public Student? Student { get; set; }
        public List<UserRole> UserRoles { get; set; } = new List<UserRole>();

        public string GenerateJwtToken(JwtOptions jwtOptions)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Issuer = jwtOptions.Issuer,
                Audience = jwtOptions.Audience,
                SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)), SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(new Claim[]
                {
                        new Claim(ClaimTypes.NameIdentifier, this.Id.ToString()),
                        new Claim(ClaimTypes.Name, this.FirstName),
                })
            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);

            return token;
        }

        public bool VerifyPassword(string providedPassword)
        {
            var passwordHasher = new PasswordHasher<object>();
            var result = passwordHasher.VerifyHashedPassword(null, Password, providedPassword);
            if (result == PasswordVerificationResult.Failed)
                return false;
            return true;
        }
    }
}
