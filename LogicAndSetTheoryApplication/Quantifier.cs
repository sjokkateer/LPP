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
            boundVariable = char.ToLower(boundVariable);

            if (!Predicate.LOWER_CASE_ALPHABET.Contains(boundVariable))
            {
                throw new ArgumentException("An alphabetical character is required");
            }

            this.boundVariable = boundVariable;
        }

        public char GetSymbol()
        {
            return Data;
        }

        public char GetBoundVariable()
        {
            return boundVariable;
        }

        public override List<Proposition> GetVariables()
        {
            throw new NotImplementedException("Is not part of first order logic");
        }

        public override List<char> GetBoundVariables()
        {
            return LeftSuccessor.GetBoundVariables();
        }

        public override Proposition Nandify()
        {
            throw new NotImplementedException("Is not part of first order logic");
        }

        public override bool Calculate()
        {
            throw new NotImplementedException("Is not part of our implementation");
        }

        public override string ToString()
        {
            if (LeftSuccessor == null)
            {
                throw new NullReferenceException("A predicate is required to be set");
            }

            string result = $"{Data}{GetBoundVariable()}.";

            result += "(";
            result += LeftSuccessor.ToString();
            result += ")";
            
            return result;
        }

        // Override:
        // - HashCode
        // - Equals
        // - Copy
    }
}
