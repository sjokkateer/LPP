using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicAndSetTheoryApplication
{
    public class ExistentialQuantifier: Quantifier
    {
        public const char SYMBOL = '!';

        public ExistentialQuantifier(char boundVariable): base(SYMBOL, boundVariable)
        { }
    }
}
