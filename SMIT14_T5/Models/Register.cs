using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMIT14_T5.Models
{
    public class Register
    {
        public System.Guid U_ID { get; set; }
        public string UserAccount1 { get; set; }
        public string UserPWD { get; set; }
        public int DataID { get; set; }
        public System.DateTime BirthDate { get; set; }
        public string UserLastName { get; set; }
        public string UserFirstName { get; set; }
        public string EmailAddress { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }


    }
}