using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicAndSetTheoryApplication
{
    class BiImplication : BinaryConnective
    {
        public BiImplication() : base('=')
        { }

        public override bool Calculate()
        {
            // Will be true if both left and right successor are true or false
            return  LeftSuccessor.Calculate() == RightSuccessor.Calculate();
        }

        public override Proposition Copy()
        {
            BiImplication copy = new BiImplication();
            copy.LeftSuccessor = LeftSuccessor.Copy();
            copy.RightSuccessor = RightSuccessor.Copy();
            return copy;
        }
    }
}
