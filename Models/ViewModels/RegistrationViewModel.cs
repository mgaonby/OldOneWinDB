using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OldOneWinDB.Models.ViewModels
{
    public class RegistrationsViewModel
    {
        public Guid RegistrationID { get; set; }
        public string FullName { get; set; }
        public string Adres { get; set; }
        public string PhoneNo { get; set; }
        public int? DocNo { get; set; }
        public DateTime? GettingDate { get; set; }
        public DateTime? OutDeptDate { get; set; }
        public DateTime? ReturnInDeptDate { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? MustBeReadyDate { get; set; }
        public string RegName { get; set; }
        public byte? State { get; set; }
    }
}
