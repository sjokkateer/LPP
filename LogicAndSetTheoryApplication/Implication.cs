using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicAndSetTheoryApplication
{
    public class Implication : BinaryConnective
    {
        public const char SYMBOL = '>';
        public Implication() : base(SYMBOL)
        { }

        public override bool Calculate()
        {
             if (LeftSuccessor.Calculate() == true && (RightSuccessor.Calculate() == false))
             {
                return false;
             }

             return true;
        }

        public override Proposition Copy()
        {
            Implication copy = new Implication();
            copy.LeftSuccessor = LeftSuccessor.Copy();
            copy.RightSuccessor = RightSuccessor.Copy();
            
            return copy;
        }

        public override Proposition Nandify()
        {
            Negation negation = new Negation();
            negation.LeftSuccessor = LeftSuccessor;
            Disjunction disjunction = new Disjunction();
            disjunction.LeftSuccessor = negation;
            disjunction.RightSuccessor = RightSuccessor;

            return disjunction.Nandify();
        }
    }
}
