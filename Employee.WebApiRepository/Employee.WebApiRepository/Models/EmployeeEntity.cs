using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Employee.WebApiRepository.Models
{
    public class EmployeeEntity
    {
        public string id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string EmailID { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string SecurityQuestion { get; set; }
        public string SecurityAnswer { get; set; }
    }
}