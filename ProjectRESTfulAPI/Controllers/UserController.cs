using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectRESTfulAPI.Interfaces;

namespace ProjectRESTfulAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUser objuser;
        public UserController(IUser _objuser)
        {
            objuser = _objuser;
        }
        [HttpGet]
        public IEnumerable<User> Index()
        {
            return objuser.GetAllUser();
        }

        [HttpPost]
        public int Create(User data)
        {
            return objuser.AddUser(data);
        }

        [HttpGet("{id}")]
        public User Details(int id)
        {
            return objuser.GetUserData(id);
        }

        [HttpPut]
        public int Edit(User data)
        {
            return objuser.UpdateUser(data);
        }

        [HttpDelete("{id}")]
        public int Delete(int id)
        {
            return objuser.DeleteUser(id);
        }
    }
}
