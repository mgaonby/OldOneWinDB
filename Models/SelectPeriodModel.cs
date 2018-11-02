using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OldOneWinDB.Models
{
    public class SelectPeriodModel
    {
        public string BeginYear { get; set; }
        public string EndYear { get; set; }
        public SelectPeriodModel(string val)
        {
            BeginYear = val;
        }
        public SelectPeriodModel()
        {

        }
        public override string ToString()
        {
            return BeginYear;
        }
    }
}
