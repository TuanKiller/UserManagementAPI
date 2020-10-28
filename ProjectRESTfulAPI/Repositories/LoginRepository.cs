using Application.Data.Context;
using Application.Data.Models;
using ProjectRESTfulAPI.Common;
using ProjectRESTfulAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectRESTfulAPI.Repositories
{
    public class LoginRepository : ILogin
    {
        ApplicationDbContext _db = new ApplicationDbContext();

        public int CheckLogin(LoginModel loginModel)
        {
            var PasswordHash = new Hash().HashPassword(loginModel.Password);
            var check = _db.Accounts.FirstOrDefault(x => x.Username == loginModel.Username);
            if (check == null)
            {
                return 0;
            }
            else
            {
                if (check.Password == PasswordHash)
                {
                    return 1;
                }

                return 2;
            }
        }
    }
}
