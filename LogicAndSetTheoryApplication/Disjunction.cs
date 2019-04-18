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
    }
}
