using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CM_Assignment8
{
    class NoHoursRecordedException : Exception
    {
        private static string msg = "Invalid Entry--Hours must be recorded or holiday selected";
        public NoHoursRecordedException() : base(msg)
        {
        }
    }
}
