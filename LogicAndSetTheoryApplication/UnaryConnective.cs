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

        public override bool Equals(object obj)
        {
            // Means our data is the same or the obj reference
            if (!base.Equals(obj)) return false;
            if (GetType() != obj.GetType()) return false;

            UnaryConnective unaryConnective = (UnaryConnective)obj;
            return unaryConnective.LeftSuccessor.Equals(LeftSuccessor);
        }

        public override string ToString()
        {
            if (LeftSuccessor == null)
            {
                throw new NullReferenceException("The left successor should not be null!");
            }

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

        public override bool Replace(char originalVariable, char newVariable)
        {
            if (LeftSuccessor.GetType() != typeof(Proposition))
            {
                return LeftSuccessor.Replace(originalVariable, newVariable);
            }

            return false;
        }

        public override bool IsAbstractProposition()
        {
            return LeftSuccessor.IsAbstractProposition();
        }
    }
}
