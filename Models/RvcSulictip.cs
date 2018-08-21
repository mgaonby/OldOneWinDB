using System;
using System.Collections.Generic;

namespace OldOneWinDB.Models
{
    public partial class RvcSulictip
    {
        public RvcSulictip()
        {
            RvcSulic = new HashSet<RvcSulic>();
        }

        public string Ultip { get; set; }
        public string Fname { get; set; }
        public string Name { get; set; }

        public ICollection<RvcSulic> RvcSulic { get; set; }
    }
}
