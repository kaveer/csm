using Microsoft.AspNetCore.Mvc;
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

        public AuthenticationController(IAuthenticationRepository authentication)
        {
            _authentication = authentication;
        }

        [HttpGet("signup")]
        public ActionResult SignUp()
        {
            try
            {
                var token = _authentication.SignUp(new Model.Authentication() { Email = "test", Password = "test" });
                if (string.IsNullOrWhiteSpace(token))
                    return NotFound();

                // 201 Created
                return Created(string.Empty, token);
            }
            catch (Exception ex)
            {
                // 500 Internal Server Error
                return StatusCode(500, ex.Message);
            }
            
        }
    }
}
