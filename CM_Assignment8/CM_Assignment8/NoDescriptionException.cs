using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CM_Assignment8
{
    class NoDescriptionException : Exception
    {
        private static string msg = "Invalid Entry--Descriptors must be added for each record.";
        public NoDescriptionException() : base(msg)
        {
        }
    }
}
