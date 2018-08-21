using System;
using System.Collections.Generic;

namespace OldOneWinDB.Models
{
    public partial class TblDocRegistry
    {
        public Guid RegId { get; set; }
        public string RegName { get; set; }
        public Guid? ParrentId { get; set; }
        public int? IssueTerms { get; set; }
        public Guid? DeptId { get; set; }
        public string AdditionalDoc { get; set; }
        public bool? GetDataFromRvc { get; set; }
        public bool? Deleted { get; set; }
        public byte? KolList { get; set; }
        public string StatementForm { get; set; }
        public byte? PeriodType { get; set; }
    }
}
