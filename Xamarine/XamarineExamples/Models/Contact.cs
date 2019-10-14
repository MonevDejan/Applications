using System;
using System.Collections.Generic;
using System.Text;

namespace SafeSportChat.Models
{

    public class Contact
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string NameSort => LastName[0].ToString();

        //public string FullName => $"{FirstName} {LastName}";
    }
}
   