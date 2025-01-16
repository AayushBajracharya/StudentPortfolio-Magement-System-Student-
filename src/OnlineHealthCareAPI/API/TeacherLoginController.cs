using Application.Dto.Patient;
using Application.Interfaces.Repositories.PatientRepository;
using Application.Interfaces.Repositories.TeacherRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API
{
    [Route("teacher-login")]
    [ApiController]
    public class TeacherLoginController : BaseApiController
    {
        private readonly IConfiguration _configuration;
        private readonly ITeacherRepository _teacherRepository;

        public TeacherLoginController(IConfiguration configuration, ITeacherRepository teacherRepository)
        {
            _configuration = configuration;
            _teacherRepository = teacherRepository;
        }

        private string GenerateToken(LoginDto user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var authClaims = new[]
            {
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: authClaims,
                expires: DateTime.Now.AddMinutes(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login(LoginDto user)
        {
            var authenticatedUser = AuthenticateUser(user, out var teacherId);
            if (authenticatedUser != null)
            {
                var token = GenerateToken(authenticatedUser);
                return Ok(new { token, message = "Logged in successfully", teacherId });
            }
            else
            {
                return Unauthorized(new { message = "Email or password is incorrect" });
            }
        }

        private LoginDto AuthenticateUser(LoginDto user, out int teacherId)
        {
            teacherId = 0;
            var hashedPassword = HashPassword(user.Password);
            var teacher = _teacherRepository.Queryable
                              .FirstOrDefault(x => x.Email == user.Email && x.Password == hashedPassword);

            if (teacher != null)
            {
                teacherId = teacher.Id;
                return new LoginDto
                {
                    Email = teacher.Email
                };
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
