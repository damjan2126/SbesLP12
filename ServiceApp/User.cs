using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceApp
{
    enum UserRole 
    {
        ASISTENT,
        STUDENT
    }
    internal class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public UserRole Role { get; set; }

        internal User() { }

        internal User(int id, string firstName, string lastName, string username, UserRole role)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Username = username;
            Role = role;
        }
    }
}
