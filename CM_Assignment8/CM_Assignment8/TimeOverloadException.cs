using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CM_Assignment8
{
    class TimeOverloadException : Exception
    {
        private static string msg = "Invalid Entry--Only 24 hours allowed per day.";
        public TimeOverloadException() : base(msg)
        {
        }
    }
}
