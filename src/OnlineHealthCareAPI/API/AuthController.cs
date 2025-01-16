using Application.Dto.Patient;
using Application.Interfaces.Repositories.PatientRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API
{
    [Route("")]
    [ApiController]
    public class AuthController : BaseApiController
    {
        private readonly IConfiguration _configuration;
        private readonly IPatientRepository _patientRepository;

        public AuthController(IConfiguration configuration,
            IPatientRepository patientRepository)
        {

            _configuration = configuration;
            _patientRepository = patientRepository;
        }

        private string GenerateToken(LoginDto user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var authClaims = new[]
            {
                new Claim(ClaimTypes.Name,user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: authClaims,
                expires: DateTime.Now.AddMinutes(1),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        [AllowAnonymous]
        [HttpPost("Logins")]
        public IActionResult Login(LoginDto user)
        {
            var authenticatedUser = AuthenticateUser(user, out var patientId);
            if (authenticatedUser != null)
            {
                var token = GenerateToken(authenticatedUser);
                return Ok(new { token, message = "Logged in successfully", patientId });
            }
            else
            {
                return Unauthorized(new { message = "Email or password is incorrect" });
            }
        }

        private LoginDto AuthenticateUser(LoginDto user, out int patientId)
        {
            patientId = 0;
            var hashedPassword = HashPassword(user.Password);
            var patient = _patientRepository.Queryable
                               .FirstOrDefault(x => x.EmailAddress == user.Email && x.Password == hashedPassword);

            if (patient != null)
            {
                patientId = patient.Id;
                var authenticatedUser = new LoginDto
                {
                    Email = patient.EmailAddress,

                };


                return authenticatedUser;
            }

            return null;
        }

        private string HashPassword(string password)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
