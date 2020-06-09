using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicAndSetTheoryApplication
{
    public class Negation : UnaryConnective
    {
        public const char SYMBOL = '~';

        public Negation() : base(SYMBOL)
        { }

        public override bool Calculate()
        {
            // Return the negation of the child proposition.
            return !(LeftSuccessor.Calculate());
        }

        public override Proposition Copy()
        {
            Negation copy = new Negation();
            copy.LeftSuccessor = LeftSuccessor.Copy();
            
            return copy;
        }

        public override Proposition Nandify()
        {
            // ~(A) == ~(A & A) == A % A
            Nand nand = new Nand();
            Proposition nandifiedLeft = LeftSuccessor;
            
            if (LeftSuccessor.GetType() != typeof(Proposition))
            {
                nandifiedLeft = LeftSuccessor.Nandify();
            }
            
            nand.LeftSuccessor = nandifiedLeft;
            nand.RightSuccessor = nandifiedLeft;
            
            return nand;
        }
    }
}
