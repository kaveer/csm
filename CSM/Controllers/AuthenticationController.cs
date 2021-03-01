using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Model;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSM.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationRepository _authentication;
        private IConfiguration _configuration;

        public AuthenticationController(IAuthenticationRepository authentication, IConfiguration configuration)
        {
            _authentication = authentication;
            _configuration = configuration;
        }

        [HttpPost("signup")]
        public ActionResult SignUp(Authentication item)
        {
            try
            {
                var token = _authentication.SignUp(new Model.Authentication() { Email = "test2", Password = "test" });
                if (string.IsNullOrWhiteSpace(token))
                    return Unauthorized();

                // 201 Created
                return Created(_configuration["Environment:Dev:Url"].ToString(), token);
            }
            catch (Exception ex)
            {
                // 500 Internal Server Error
                return StatusCode(500, ex.Message);
            }
            
        }
    }
}
