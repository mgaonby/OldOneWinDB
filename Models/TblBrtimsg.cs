using System;
using System.Collections.Generic;

namespace OldOneWinDB.Models
{
    public partial class TblBrtimsg
    {
        public Guid BrtimsgId { get; set; }
        public Guid RegistrationId { get; set; }
        public string Fname { get; set; }
        public string Mname { get; set; }
        public string Lname { get; set; }
        public DateTime? Dob { get; set; }
        public string DocType { get; set; }
        public string DocNo { get; set; }
        public string PersonalNo { get; set; }
        public DateTime? DocIssueDate { get; set; }
        public string DocIssuer { get; set; }
        public string Address { get; set; }
        public DateTime? AddressDate { get; set; }
        public string PayNo { get; set; }
        public int? Summ { get; set; }
        public string Bank { get; set; }
        public byte Sent { get; set; }
        public byte InterDoc { get; set; }
        public string DogNo { get; set; }
        public DateTime? DogDate { get; set; }
    }
}
