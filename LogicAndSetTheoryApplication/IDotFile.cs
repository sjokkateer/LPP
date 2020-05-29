using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicAndSetTheoryApplication
{
    interface IDotFile
    {
        int NodeNumber { get; set; }
        string NodeLabel();
    }
}
