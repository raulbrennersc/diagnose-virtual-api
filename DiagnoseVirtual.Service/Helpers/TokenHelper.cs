﻿using DiagnoseVirtual.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DiagnoseVirtual.Service.Helpers
{
    public static class TokenHelper
    {
        public static string GerarTokenUsuario(Usuario usuario, string tokenKey)
        {
            var claims = new[]
            {
                new Claim("IdUsuario", usuario.Id.ToString()),
                new Claim("NomeUsuario", usuario.Nome),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds,
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
