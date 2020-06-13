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
        public SemanticTableauxElement Head { get; }

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

            // Then the type of semantic tableaux element could be
            // set by the semantic tableaux by perhaps setting
            // an enum on the semantic tableaux element
            // which calls a create method
            SemanticTableauxElement.ResetReplacementVariables();
            Head = new SemanticTableauxElement(propositions);
        }

        public bool IsClosed()
        {
            return Head.IsClosed();
        }
    }
}
