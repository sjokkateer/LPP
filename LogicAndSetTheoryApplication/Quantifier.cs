using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicAndSetTheoryApplication
{
    public abstract class Quantifier: UnaryConnective
    {
        private char boundVariable;

        // symbol of the quantifier plus a bound variable for all x or there exists a z etc.
        public Quantifier(char symbol, char boundVariable): base(symbol)
        {
            this.boundVariable = boundVariable;
        }

        // This should not have:
        // - A getVariables method (this is only used for abstract propositions)
        // - A Nandify method
        // - A Calculate method 

        // Override:
        // - HashCode
        // - Equals
        // - Copy
    }
}
