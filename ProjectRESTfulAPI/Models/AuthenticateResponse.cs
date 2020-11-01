using Application.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProjectRESTfulAPI.Models
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool Status { get; set; }
        public string Username { get; set; }
        public string JwtToken { get; set; }

        [JsonIgnore] // refresh token is returned in http only cookie
        public string RefreshToken { get; set; }
        public AuthenticateResponse(Account account, string jwtToken, string refreshToken)
        {
            Id = account.Id;
            Username = account.Username;
            Email = account.Email;
            Status = account.Status;
            Username = account.Username;
            JwtToken = jwtToken;
            RefreshToken = refreshToken;
        }
    }

}
