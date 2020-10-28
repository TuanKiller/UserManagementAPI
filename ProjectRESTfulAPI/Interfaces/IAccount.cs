using Application.Data.Entities;
using Application.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectRESTfulAPI.Interfaces
{
    public interface IAccount
    {
        List<Account> GetAll();
        bool Register(AccountModel account);
    }
}
