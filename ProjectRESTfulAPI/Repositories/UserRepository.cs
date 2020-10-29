using Application.Data.Context;
using Application.Data.Entities;
using Microsoft.EntityFrameworkCore;
using ProjectRESTfulAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectRESTfulAPI.Repositories
{
    public class UserRepository : IUser
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public IEnumerable<User> GetAllUser()
        {
            try
            {
                return db.Users.ToList().OrderBy(x => x.Id);
            }
            catch
            {
                throw;
            }
        }
        //To add new User record
        public int AddUser(User data)
        {
            try
            {
                db.Users.Add(data);
                db.SaveChanges();
                return 1;
            }
            catch
            {
                throw;
            }
        }
        //To Update the records of a particluar User
        public int UpdateUser(User data)
        {
            try
            {
                db.Entry(data).State = EntityState.Modified;
                db.SaveChanges();

                return 1;
            }
            catch
            {
                throw;
            }
        }
        //Get the details of a particular User
        public User GetUserData(int id)
        {
            try
            {
                User data = db.Users.Find(id);
                return data;
            }
            catch
            {
                throw;
            }
        }

        //To Delete the record on a particular User
        public int DeleteUser(int id)
        {
            try
            {
                User stu = db.Users.Find(id);
                db.Users.Remove(stu);
                db.SaveChanges();
                return 1;
            }
            catch
            {
                throw;
            }
        }
    }
}
