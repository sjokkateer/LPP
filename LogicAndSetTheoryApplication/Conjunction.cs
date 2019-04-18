using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicAndSetTheoryApplication
{
    class Conjunction : BinaryConnective
    {
        public Conjunction() : base('&')
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
    }
}
