using BusinessLogic.DTOs.SystemAccountDTOs;
using BusinessLogic.Interfaces;
using Data.Entities;
using Data.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;


namespace BusinessLogic.Services
{
    public class SystemAccountService : ISystemAccountService
    {
        private readonly IConfiguration _configuration;
        private readonly IUOW _uow;

        public SystemAccountService(IConfiguration configuration, IUOW uow)
        {
            _configuration = configuration;
            _uow = uow;
        }

        public async Task<string> Login(LoginDTO loginDto)
        {
            var userRepository = _uow.GetRepository<User>();

            var user = userRepository.Entities
                                     .Include(u => u.Role)
                                     .FirstOrDefault(u => u.Email == loginDto.AccountEmail);

            if ((user == null || user.DeletedTime.HasValue) || !BCrypt.Net.BCrypt.Verify(loginDto.AccountPassword, user.Password))
            {
                return null;
            }



            // Generate and return a JWT token if credentials are valid.
            return GenerateJwtToken(user);
        }
        public async Task<bool> Register(RegisterDTO registerDto)
        {
            var userRepository = _uow.GetRepository<User>();

            // Check if the email is already registered.
            var existingUser = userRepository.Entities.FirstOrDefault(u => u.Email == registerDto.Email);
            if (existingUser != null)
            {
                // User with this email already exists.
                return false;
            }
            // Get the "Customer" role.
            var roleRepository = _uow.GetRepository<Role>();
            var customerRole = roleRepository.Entities.FirstOrDefault(r => r.Name == "Customer");
            if (customerRole == null)
            {
                throw new Exception("Customer role not found. Please ensure the Customer role exists.");
            }

            // Hash the password using BCrypt.
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            // Create a new user entity.
            var newUser = new User
            {
                Email = registerDto.Email,
                Name = registerDto.Name,
                Password = hashedPassword,
                PhoneNumber = registerDto.PhoneNumber,
                Address = registerDto.Address,
                // If a RoleId is provided, assign it; otherwise, it can be set later or defaulted.
                RoleId = customerRole.Id,


            };

            // Insert the new user.
            await userRepository.InsertAsync(newUser);
            await _uow.SaveAsync();

            return true;
        }

        private string GenerateJwtToken(User user)
        {
            var secret = _configuration["Jwt:SecretKey"];
            if (string.IsNullOrEmpty(secret))
            {
                throw new Exception("JWT SecretKey is not configured.");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role != null ? user.Role.Name : "User")
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
