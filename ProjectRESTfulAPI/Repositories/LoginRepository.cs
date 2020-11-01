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
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ProjectRESTfulAPI.Repositories
{
    public class LoginRepository : ILogin
    {
        private readonly AppSettings _appSettings;
        ApplicationDbContext _db = new ApplicationDbContext();
        public LoginRepository(ApplicationDbContext db,IOptions<AppSettings> appSettings)
        {
            _db = db;
            _appSettings = appSettings.Value;
        }
        public AuthenticateResponse Authenticate(AuthenticateRequest model, string ipAddress)
        {
            var account = _db.Accounts.SingleOrDefault(x => x.Username == model.Username && x.Password == model.Password);

            // return null if user not found
            if (account == null) return null;

            // authentication successful so generate jwt and refresh tokens
            var jwtToken = generateJwtToken(account);
            var refreshToken = generateRefreshToken(ipAddress);

            // save refresh token
            account.RefreshTokens.Add(refreshToken);
            _db.Update(account);
            _db.SaveChanges();

            return new AuthenticateResponse(account, jwtToken, refreshToken.Token);
        }
        public AuthenticateResponse RefreshToken(string token, string ipAddress)
        {
            var account = _db.Accounts.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));

            // return null if no user found with token
            if (account == null) return null;

            var refreshToken = account.RefreshTokens.Single(x => x.Token == token);

            // return null if token is no longer active
            if (!refreshToken.IsActive) return null;

            // replace old refresh token with a new one and save
            var newRefreshToken = generateRefreshToken(ipAddress);
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.ReplacedByToken = newRefreshToken.Token;
            account.RefreshTokens.Add(newRefreshToken);
            _db.Update(account);
            _db.SaveChanges();

            // generate new jwt
            var jwtToken = generateJwtToken(account);

            return new AuthenticateResponse(account, jwtToken, newRefreshToken.Token);
        }
        public bool RevokeToken(string token, string ipAddress)
        {
            var account = _db.Accounts.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));

            // return false if no user found with token
            if (account == null) return false;

            var refreshToken = account.RefreshTokens.Single(x => x.Token == token);

            // return false if token is not active
            if (!refreshToken.IsActive) return false;

            // revoke token and save
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            _db.Update(account);
            _db.SaveChanges();

            return true;
        }
        public IEnumerable<Account> GetAll()
        {
            return _db.Accounts;
        }
        public Account GetById(int id)
        {
            return _db.Accounts.FirstOrDefault(x => x.Id == id);
        }
        // helper methods

        private string generateJwtToken(Account account)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, account.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        private RefreshToken generateRefreshToken(string ipAddress)
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                return new RefreshToken
                {
                    Token = Convert.ToBase64String(randomBytes),
                    Expires = DateTime.UtcNow.AddDays(7),
                    Created = DateTime.UtcNow,
                    CreatedByIp = ipAddress
                };
            }
        }
    }
}
