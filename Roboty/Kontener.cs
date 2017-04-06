using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roboty
{
    class Kontener
    {
        public int LogID { get; set; }
        public string Date { get; set; }
        public string Describe { get; set; }

        public Kontener(int nLogID, DateTime dDate, string sDescribe)
        {
            string temp = dDate.ToString("HH:mm:ss d/M/yyyy");
            Date = temp;
            LogID = nLogID;
            Describe = sDescribe;
        }
    }
}
