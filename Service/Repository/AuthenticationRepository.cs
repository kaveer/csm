using Model;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Repository
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        public string SignUp(Authentication item)
        {
            return "test";
            
        }
    }
}
