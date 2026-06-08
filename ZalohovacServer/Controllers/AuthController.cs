using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ZalohovacServer.Database;
using ZalohovacServer.Entities.API;
using ZalohovacServer.Entities.DB;

namespace ZalohovacServer.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IConfiguration _configuration;
        private DatabaseContext _context;

        public AuthController(IConfiguration configuration, DatabaseContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        // POST api/auth/login
        [HttpPost("login")]
        public ActionResult<Token> Login([FromBody] Credentials credentials)
        {
            Account? account = _context.Accounts.SingleOrDefault(a => a.Username == credentials.Username);

            if (account == null || !BCrypt.Net.BCrypt.Verify(credentials.Password, account.Password))
                return Unauthorized("Špatné jméno nebo heslo.");

            string issuer = _configuration["Jwt:Issuer"]!;
            string key = _configuration["Jwt:Key"]!;

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Iss, issuer),
                    new Claim(JwtRegisteredClaimNames.Aud, issuer),
                    new Claim(JwtRegisteredClaimNames.Sub, account.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Name, account.Username)
                }),
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            SecurityToken tokenObject = tokenHandler.CreateToken(tokenDescriptor);
            string tokenString = tokenHandler.WriteToken(tokenObject);

            return Ok(new Token(tokenString));
        }
    }
}
