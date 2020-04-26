using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicAndSetTheoryApplication
{
    public class Conjunction : BinaryConnective
    {
        public const char SYMBOL = '&';
        public Conjunction() : base(SYMBOL)
        { }

        public override bool Calculate()
        {
            return LeftSuccessor.Calculate() && RightSuccessor.Calculate();
        }

        public override Proposition Copy()
        {
            Conjunction copy = new Conjunction();
            copy.LeftSuccessor = LeftSuccessor.Copy();
            copy.RightSuccessor = RightSuccessor.Copy();
            return copy;
        }

        public override Proposition Nandify()
        {
            // P & Q == ~(~(P & Q))
            // Now ~(P & Q) == (P % Q)
            // And finally ~(P % Q) == (P % Q) % (P % Q)
            // Create the double negation equivalent of the conjunction.
            Negation negation = new Negation();
            Nand nand = new Nand();
            nand.LeftSuccessor = LeftSuccessor;
            nand.RightSuccessor = RightSuccessor;
            negation.LeftSuccessor = nand;
            return negation.Nandify();
        }
    }
}
