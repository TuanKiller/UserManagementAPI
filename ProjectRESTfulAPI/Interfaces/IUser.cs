using Application.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectRESTfulAPI.Interfaces
{
    public interface IUser
    {
        IEnumerable<User> GetAllUser();
        int AddUser(User data);
        int UpdateUser(User data);
        User GetUserData(int id);
        int DeleteUser(int id);
    }
}
