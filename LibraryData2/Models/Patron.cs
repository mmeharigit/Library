using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryData2.Models
{
    public class Patron
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string TelephoneNumber { get; set; }

       public virtual LibraryCard LibraryCard { get; set; }
       public virtual LibraryBranch HomeLibraryBranch { get; set; }

        //we use virtual to lazy load that library card when ever we want to

    }
}
