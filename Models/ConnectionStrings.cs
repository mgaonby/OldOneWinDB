using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OldOneWinDB.Models
{
    public class ConnectionStrings
    {
        public string AreaName { get; set; }
        public string ConnectionString { get; set; }
        public ConnectionStrings()
        {

        }
        public ConnectionStrings(string areaName, string connectionString)
        {
            AreaName = areaName;
            ConnectionString = connectionString;
        }
        public override string ToString()
        {
            return AreaName;
        }
    }
}
