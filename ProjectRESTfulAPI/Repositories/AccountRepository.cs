using Application.Data.Context;
using Application.Data.Entities;
using Application.Data.Models;
using ProjectRESTfulAPI.Common;
using ProjectRESTfulAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectRESTfulAPI.Repositories
{
    public class AccountRepository : IAccount
    {
        ApplicationDbContext _db = new ApplicationDbContext();
        public AccountRepository()
        {
        }
        public List<Account> GetAll()
        {
            return _db.Accounts.ToList();
        }

        public bool Register(AccountModel accountModel)
        {
            var model = _db.Accounts.FirstOrDefault(x => x.Username == accountModel.Username);
            if (model == null)
            {
                Account account = new Account()
                {
                    Username = accountModel.Username,
                    Password = new Hash().HashPassword(accountModel.Password),
                    Name = accountModel.Name,
                    Email = accountModel.Email,
                    Status = accountModel.Status
                };
                _db.Accounts.Add(account);
                _db.SaveChanges();
                return true;
            }

            return false;
        }
    }
}
