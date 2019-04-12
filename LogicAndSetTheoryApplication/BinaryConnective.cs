using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicAndSetTheoryApplication
{
    class BinaryConnective : UnaryConnective
    {
        public Symbol RightSuccessor { get; set; }

        public BinaryConnective(object data) : base(data)
        {
            RightSuccessor = null;
        }

        public override string ToString()
        {
            string result = "(";
            result += LeftSuccessor;
            result += $" {Data} ";
            result += RightSuccessor;
            result += ")";
            return result;
        }

        public override string NodeLabel()
        {
            string result = base.NodeLabel();
            result += $"\tnode{NodeNumber} -- node{RightSuccessor.NodeNumber}\n";
            result += RightSuccessor.NodeLabel();
            return result;
        }
    }
}
