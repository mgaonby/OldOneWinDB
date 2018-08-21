using System;
using System.Collections.Generic;

namespace OldOneWinDB.Models
{
    public partial class RvcSulic
    {
        public RvcSulic()
        {
            RvcBdom = new HashSet<RvcBdom>();
        }

        public string Kodul { get; set; }
        public string Ultip { get; set; }
        public string Name { get; set; }

        public RvcSulictip UltipNavigation { get; set; }
        public ICollection<RvcBdom> RvcBdom { get; set; }
    }
}
