using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicAndSetTheoryApplication
{
    abstract public class Constant: Proposition
    {
        public Constant(char symbol): base(symbol)
        { }
    }
}
