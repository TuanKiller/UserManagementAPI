using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Data.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Office { get; set; }
        public bool Gender { get; set; }
        public DateTime DayOfBirthday { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }
    }
}
