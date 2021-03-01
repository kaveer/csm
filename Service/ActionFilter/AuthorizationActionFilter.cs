using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Model;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;

namespace Service.ActionFilter
{
    public class AuthorizationActionFilter : IAuthorizationFilter
    {
        private IConfiguration _configuration;

        public AuthorizationActionFilter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnAuthorization(AuthorizationFilterContext authcontext)
        {
            string authHeader = authcontext.HttpContext.Request.Headers["Authorization"];
            if (string.IsNullOrWhiteSpace(authHeader))
                throw new Exception("Unauthorised to access resource.");

            var stream = authHeader;
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = handler.ReadToken(stream) as JwtSecurityToken;

            var email = tokenS.Claims.FirstOrDefault(claim => claim.Type == "email").Value;
            var nameid = tokenS.Claims.FirstOrDefault(claim => claim.Type == "nameid").Value;
            int userId = 0;

            if (!string.IsNullOrWhiteSpace(email) && !int.TryParse(nameid, out userId))
                throw new Exception("Unauthorised to access resource.");

            if (userId <= 0)
                throw new Exception("Unauthorised to access resource.");

            using var context = new CsmContext(this._configuration.GetConnectionString("CsmConnection"));
            var exist = context.authentications
                                    .Where(x => x.Email == email && x.UserId == userId)
                                    .FirstOrDefault();

            if (exist == null)
                throw new Exception("Unauthorised to access resource.");

        }
    }
}
