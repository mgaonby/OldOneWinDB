using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OldOneWinDB.Models
{
    public class SelectPeriodModel
    {
        public string Year { get; set; }
        public SelectPeriodModel(string val)
        {
            Year = val;
        }
        public SelectPeriodModel()
        {

        }
        public override string ToString()
        {
            return Year;
        }
    }
}
