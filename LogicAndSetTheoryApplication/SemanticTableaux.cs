using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicAndSetTheoryApplication
{
    public class SemanticTableaux
    {
        public Proposition Proposition { get; }
        private SemanticTableauxElement semanticTableauxElement;

        public SemanticTableaux(Proposition proposition)
        {
            Proposition = proposition;

            if (proposition == null)
            {
                throw new NullReferenceException("A proposition is required to make use of the semantic tableaux");
            }

            Negation negation = new Negation();
            negation.LeftSuccessor = proposition;

            HashSet<Proposition> propositions = new HashSet<Proposition>()
            {
                negation
            };

            semanticTableauxElement = new SemanticTableauxElement(propositions);
        }

        public bool IsClosed()
        {
            return semanticTableauxElement.IsClosed();
        }
    }
}
