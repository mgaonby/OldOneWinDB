using System;
using System.Collections.Generic;

namespace OldOneWinDB.Models
{
    public partial class TblOrganization
    {
        public Guid DeptId { get; set; }
        public string DeptName { get; set; }
        public string Address { get; set; }
        public string Cabinet { get; set; }
        public string PhoneNo { get; set; }
        public string Notes { get; set; }
    }
}
