using System;
using System.Collections.Generic;

namespace OldOneWinDB.Models
{
    public partial class TblFamily
    {
        public Guid FamilyId { get; set; }
        public Guid RegistrationId { get; set; }
        public string Fname { get; set; }
        public string Mname { get; set; }
        public string Lname { get; set; }
        public DateTime Dob { get; set; }
        public string NrotN { get; set; }
        public string Address { get; set; }
        public DateTime? AddressDate { get; set; }
    }
}
