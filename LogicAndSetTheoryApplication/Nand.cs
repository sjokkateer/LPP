using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicAndSetTheoryApplication
{
    class Nand : BinaryConnective
    {
        public Nand() : base('%')
        { }

        public override bool Calculate()
        {
            return !(LeftSuccessor.Calculate() && RightSuccessor.Calculate());
        }

        public override Proposition Copy()
        {
            Nand newBase = new Nand();
            newBase.LeftSuccessor = LeftSuccessor.Copy();
            newBase.RightSuccessor = RightSuccessor.Copy();
            return newBase;
        }

        public override Proposition Nandify()
        {
            if (LeftSuccessor.GetType() != typeof(Proposition))
            {
                LeftSuccessor = LeftSuccessor.Nandify();
            }

            if (RightSuccessor.GetType() != typeof(Proposition))
            {
                RightSuccessor = RightSuccessor.Nandify();
            }
            return this;
        }
    }
}
