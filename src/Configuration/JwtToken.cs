using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MeuTodo.Models;
using Microsoft.IdentityModel.Tokens;
using System;

namespace MeuTodo.Configuration
{
    public static class JwtToken
    {
        public static string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.Name, user.Name)
                    }),
                Expires = (DateTime.Now.AddMinutes(5).ToUniversalTime()),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Settings.Key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}