using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Data.Entities;
using Application.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectRESTfulAPI.Interfaces;

namespace ProjectRESTfulAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccount _account;
        public AccountController(IAccount account)
        {
            _account = account;
        }
        // GET: api/Account
        [HttpGet]
        public IActionResult Get()
        {
            List<Account> list = _account.GetAll();
            return Ok(list);
        }

        // GET: api/Account/5
        [HttpGet("{id}", Name = "GetAccount")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Account
        [HttpPost]
        public bool Post(AccountModel accountModel)
        {
            var check = _account.Register(accountModel);
            return check;
        }

        // PUT: api/Account/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
