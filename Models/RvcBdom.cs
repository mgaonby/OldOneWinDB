using System;
using System.Collections.Generic;

namespace OldOneWinDB.Models
{
    public partial class RvcBdom
    {
        public string Koddom { get; set; }
        public string Jreo { get; set; }
        public string Jes { get; set; }
        public string Postav { get; set; }
        public string Kodul { get; set; }
        public int? Ndom { get; set; }
        public string Korp { get; set; }
        public string Ind { get; set; }
        public string Fond { get; set; }
        public string Kmst { get; set; }
        public string Krkm { get; set; }
        public double? Plob { get; set; }
        public double? Plpl { get; set; }
        public double? Plgl { get; set; }
        public int? Kolkv { get; set; }
        public string Etag { get; set; }
        public double? Godpost { get; set; }
        public double? Kopl { get; set; }
        public string Datekor { get; set; }
        public string Username { get; set; }
        public string AdresDop { get; set; }
        public double? Plzem { get; set; }

        public RvcSulic KodulNavigation { get; set; }
    }
}
