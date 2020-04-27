using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicAndSetTheoryApplication
{
    public class UnaryConnective : Proposition
    {
        public Proposition LeftSuccessor { get; set; }

        public UnaryConnective(char data) : base(data)
        {
            LeftSuccessor = null;
        }

        public override List<Proposition> GetVariables()
        {
            return LeftSuccessor.GetVariables().Distinct().ToList();
        }

        [ExcludeFromCodeCoverage]
        public override string ToString()
        {
            string result = Convert.ToString(Data);
            if (!(LeftSuccessor is BinaryConnective))
            {
                result += "(";
                result += LeftSuccessor;
                result += ")";
            }
            else
            {
                result += LeftSuccessor;
            }
            return result;
        }

        [ExcludeFromCodeCoverage]
        public override string NodeLabel()
        {
            string result = base.NodeLabel();
            result += $"\tnode{NodeNumber} -- node{LeftSuccessor.NodeNumber}\n";
            result += LeftSuccessor.NodeLabel();
            return result;
        }
    }
}
