﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicAndSetTheoryApplication
{
    public class Nand : BinaryConnective
    {
        public const char SYMBOL = '%';
        public Nand() : base(SYMBOL)
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
