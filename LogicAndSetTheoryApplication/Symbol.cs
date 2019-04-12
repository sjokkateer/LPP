using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicAndSetTheoryApplication
{
    class Symbol
    {
        public int NodeNumber { get; set; }

        public object Data { get; }

        public Symbol(object data)
        {
            Data = data;
        }

        public override string ToString()
        {
            return Data.ToString();
        }

        public virtual string NodeLabel()
        {
            return $"\tnode{NodeNumber} [ label = \"{Data}\" ]\n";
        }
    }
}
