using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicAndSetTheoryApplication
{
    class Implication : BinaryConnective
    {
        public Implication() : base('>')
        { }

        public override bool Calculate()
        {
             if (LeftSuccessor.Calculate() == true &&  (RightSuccessor.Calculate() == false))
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
    }
}
