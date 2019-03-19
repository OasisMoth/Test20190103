using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileAssortment
{
    public class AssortCompleteEventArgs : EventArgs
    {
        public List<Exception> ErrorHolder = new List<Exception>();
    }
}
