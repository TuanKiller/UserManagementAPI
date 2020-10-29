using Application.Data.Entities;
using ProjectRESTfulAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectRESTfulAPI.Interfaces
{
    public interface ILogin
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        IEnumerable<Account> GetAll();
        Account GetById(int id);
    }
}
