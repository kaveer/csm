using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Interface
{
    public interface IAuthenticationRepository
    {
        string SignUp(Authentication item);

        string LogIn(Authentication item);
       
    }
}
