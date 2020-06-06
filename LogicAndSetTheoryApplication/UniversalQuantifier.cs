using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicAndSetTheoryApplication
{
    public class UniversalQuantifier: Quantifier
    {
        public const char SYMBOL = '@';

        public UniversalQuantifier(char boundVariable): base(SYMBOL, boundVariable)
        { }


    }
}
