﻿using Demo.Web.API.DatabaseContext;
using Demo.Web.API.Interfaces;
using Demo.Web.API.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Demo.Web.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public AuthService(IConfiguration configuration, ApplicationDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }
        public string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role,user.Role)
            };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public User ValidateUser(string UserDTOEmail, string UserDTOPassword)
        {
            var user = _context.Users.Where(user => user.Email == UserDTOEmail &&
            user.Password == UserDTOPassword).FirstOrDefault();

            if (user != null)
            {
                return user;
            }
            else
            {
                return null;
            }
        }

        public SecurityToken ValidateToken(string token)
        {
            if (token != null)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                try
                {
                    tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = _configuration["Jwt:Issuer"],
                        ValidAudience = _configuration["Jwt:Audience"],
                        IssuerSigningKey = securityKey,
                    }, out SecurityToken validatedToken);

                    return validatedToken;
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }

        public string GetUserRole(SecurityToken validatedToken)
        {
            var token = (JwtSecurityToken)validatedToken;
            if (token != null)
            {
                var role = token.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
                return role;
            }
            return null;
        }

        public string RefreshToken(SecurityToken validatedToken)
        {
            //var validatedToken = ValidateToken(jwtToken.ToString());
            var token = (JwtSecurityToken)validatedToken;
            //we use a validated token
            if (token != null)
            {
                //if token is valid user is authenticated
                var userEmail = token.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
                var user = _context.Users.Where(user => user.Email == userEmail).FirstOrDefault();

                //we generate a new token with 60min of duration
                return GenerateToken(user);
            }
            return null;
        }
    }
}
