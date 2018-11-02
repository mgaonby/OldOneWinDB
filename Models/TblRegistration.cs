using OldOneWinDB.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OldOneWinDB.Models
{
    public partial class TblRegistration
    {
        public Guid RegistrationId { get; set; }
        public string Fname { get; set; }
        public string Mname { get; set; }
        public string Lname { get; set; }
        public string Address { get; set; }
        public int? Home { get; set; }
        public string Flat { get; set; }
        public string PhoneNo { get; set; }
        public string PassportNo { get; set; }
        public DateTime? PassIssueDate { get; set; }
        public string PassIssuer { get; set; }
        public string PersonalNo { get; set; }
        public Guid? RegId { get; set; }
        public byte? Registrator { get; set; }
        public DateTime? GettingDate { get; set; }
        public DateTime? OutDeptDate { get; set; }
        public DateTime? ReturnInDeptDate { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? MustBeReadyDate { get; set; }
        public bool? ResultType { get; set; }
        public bool Deleted { get; set; }
        public byte? State { get; set; }
        public string Notes { get; set; }
        public byte[] Rvccontent { get; set; }
        public int OrderNo { get; set; }
        public int? DocNo { get; set; }
        public string Nprav { get; set; }
        public decimal? PlO { get; set; }
        public decimal? PlG { get; set; }
        public byte? KolB { get; set; }
        public string Organiz { get; set; }
        public byte? Vid { get; set; }
        public byte? Room { get; set; }
        public byte? KolList { get; set; }
        public byte? KolListPril { get; set; }
        public string StatementForm { get; set; }
        public string Proceedings { get; set; }
        public DateTime? DateSsolutions { get; set; }
        public string NumberSolutions { get; set; }
        public string EvaluationNotification { get; set; }
        public string EvaluationControl { get; set; }
        public string CaseNumber { get; set; }
        public byte? KolListCase { get; set; }

        
        public virtual TblDocRegistry TblDocRegistry { get; set; }


        public RegistrationsViewModel GetRegistrationsViewModel()
        {
            return new RegistrationsViewModel
            {
                Adres = Address,
                FullName = String.Format("{0} {1}. {2}.", this.Lname, this.Fname.Length != 0 ? this.Fname[0].ToString() : "", this.Mname.Length != 0 ? this.Mname[0].ToString() : ""),
                GettingDate = this.GettingDate,
                IssueDate = this.IssueDate,
                MustBeReadyDate = this.MustBeReadyDate,
                OutDeptDate = this.OutDeptDate,
                PhoneNo = this.PhoneNo,
                RegistrationID = this.RegistrationId,
                ReturnInDeptDate = this.ReturnInDeptDate,
                State = this.State,
                DocNo = this.DocNo,
                ResultType = this.ResultType
            };
        }
    }
}
