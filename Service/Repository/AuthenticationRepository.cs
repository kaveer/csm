using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Model;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Service.Repository
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private IConfiguration _configuration;

        public AuthenticationRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string SignUp(Authentication item)
        {
            if (!ValidateModel(item))
                throw new Exception("Invalid credential");

            using var context = new CsmContext(this._configuration.GetConnectionString("CsmConnection"));
            var exist = context.authentications
                                    .Where(x => x.Email == item.Email)
                                    .FirstOrDefault();
            if (exist != null)
                throw new Exception("User already exist.");

            item.Password = EncryptPassword(item.Password);
            context.authentications.Add(item);
            context.SaveChanges();

            if (item.UserId == 0)
                throw new Exception("Fail to sign up");

            return GenerateToken(item);

        }

        public string LogIn(Authentication item)
        {
            if (!ValidateModel(item))
                throw new Exception("Invalid credential");

            using var context = new CsmContext(this._configuration.GetConnectionString("CsmConnection"));
            var exist = context.authentications
                                    .Where(x => x.Email == item.Email && x.Password == EncryptPassword(item.Password))
                                    .FirstOrDefault();
            if (exist == null)
                throw new Exception("User does not exist.");

            return GenerateToken(exist);
        }

        private string GenerateToken(Authentication item)
        {
            string secret = _configuration["TokenSetting:SecretHash"];
            string expireMinutes = _configuration["TokenSetting:ExpireMinutes"];

            var symmetricKey = Convert.FromBase64String(secret);
            var tokenHandler = new JwtSecurityTokenHandler();

            var now = DateTime.UtcNow;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, item.Email),
                    new Claim(ClaimTypes.NameIdentifier, item.UserId.ToString())
                }),

                Expires = now.AddMinutes(Convert.ToInt32(expireMinutes)),

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var stoken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(stoken);

            return token;
        }

        private string EncryptPassword(string password)
        {
            byte[] data = System.Text.Encoding.ASCII.GetBytes(password);
            data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
            String hash = System.Text.Encoding.ASCII.GetString(data);

            return hash;
        }

        private bool ValidateModel(Authentication item)
        {
            if (item == null)
                return false;
            if (string.IsNullOrWhiteSpace(item.Email))
                return false;
            if (string.IsNullOrWhiteSpace(item.Password))
                return false;

            return true;
        }

       
    }
}
