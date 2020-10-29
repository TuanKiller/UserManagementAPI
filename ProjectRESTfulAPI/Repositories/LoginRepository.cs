using Application.Data.Context;
using Application.Data.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProjectRESTfulAPI.Common;
using ProjectRESTfulAPI.Helper;
using ProjectRESTfulAPI.Interfaces;
using ProjectRESTfulAPI.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProjectRESTfulAPI.Repositories
{
    public class LoginRepository : ILogin
    {
        private readonly AppSettings _appSettings;
        ApplicationDbContext _db = new ApplicationDbContext();
        public LoginRepository(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }
        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var account = _db.Accounts.SingleOrDefault(x => x.Username == model.Username && x.Password == model.Password);

            // return null if user not found
            if (account == null) return null;

            // authentication successful so generate jwt token
            var token = generateJwtToken(account);

            return new AuthenticateResponse(account, token);
        }
        public IEnumerable<Account> GetAll()
        {
            return _db.Accounts;
        }
        public Account GetById(int id)
        {
            return _db.Accounts.FirstOrDefault(x => x.Id == id);
        }
        private string generateJwtToken(Account account)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", account.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
