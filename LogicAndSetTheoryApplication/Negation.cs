using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicAndSetTheoryApplication
{
    class Negation : UnaryConnective
    {
        public Negation() : base('¬')
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
    }
}
