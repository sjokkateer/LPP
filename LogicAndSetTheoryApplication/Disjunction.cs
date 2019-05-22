using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicAndSetTheoryApplication
{
    class Disjunction : BinaryConnective
    {
        public Disjunction() : base('|')
        { }

        public override bool Calculate()
        {
            return LeftSuccessor.Calculate() || RightSuccessor.Calculate();
        }

        public override Proposition Copy()
        {
            Disjunction copy = new Disjunction();
            copy.LeftSuccessor = LeftSuccessor.Copy();
            copy.RightSuccessor = RightSuccessor.Copy();
            return copy;
        }

        public override Proposition Nandify()
        {
            // P | Q == ~(~(P)) | ~(~(Q))
            // == ~(~(P) & ~(Q)) == ~(P) % ~(Q)
            Nand nand = new Nand();
            Negation negationOfLeftSuccessor = new Negation();
            negationOfLeftSuccessor.LeftSuccessor = LeftSuccessor;
            Negation negationOfRightSuccessor = new Negation();
            negationOfRightSuccessor.LeftSuccessor = RightSuccessor;
            nand.LeftSuccessor = negationOfLeftSuccessor;
            nand.RightSuccessor = negationOfRightSuccessor;
            return nand.Nandify();
        }
    }
}
