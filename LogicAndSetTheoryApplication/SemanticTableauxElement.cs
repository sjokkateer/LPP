using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicAndSetTheoryApplication
{
    public class SemanticTableauxElement: IDotFile
    {
        public int NodeNumber { get; set; }

        public HashSet<Proposition> Propositions { get; }
        public SemanticTableauxElement LeftChild { get; private set; }
        public SemanticTableauxElement RightChild { get; private set; }

        public SemanticTableauxElement(HashSet<Proposition> propositions)
        {
            if (propositions == null)
            {
                throw new NullReferenceException("Unexpected null argument, can not process tableaux element this way.");
            }

            Propositions = propositions;

            if (!IsClosed())
            {
                CreateChildren();
            }
        }

        public bool IsClosed()
        {
            List<Proposition> pSet = Propositions.ToList();

            for (int i = 0; i < pSet.Count - 1; i++)
            {
                for (int j = i + 1; j < pSet.Count; j++)
                {
                    if (IsContradiction(pSet[i], pSet[j]))
                    {
                        return true;
                    }
                }
            }

            bool closed = false;

            if (LeftChild != null)
            {
                closed = LeftChild.IsClosed();
            }

            if (RightChild != null)
            {
                closed = closed && RightChild.IsClosed();
            }

            return closed;
        }

        protected bool IsContradiction(Proposition proposition1, Proposition proposition2)
        {
            Negation negatedProposition = null;
            Proposition targetLiteral = null;

            if (proposition1 is Negation)
            {
                negatedProposition = (Negation)proposition1;
                targetLiteral = proposition2;
            }

            if (proposition2 is Negation)
            {
                negatedProposition = (Negation)proposition2;
                targetLiteral = proposition1;
            }

            if (negatedProposition != null)
            {
                return negatedProposition.LeftSuccessor.Equals(targetLiteral);
            }

            return false;
        }

        protected void CreateChildren()
        {
            bool childCreated = false;

            foreach (Proposition proposition in Propositions)
            {
                childCreated = TryToCreateDoubleNegation(proposition);

                if (childCreated)
                {
                    return;
                }
            }

            foreach (Proposition proposition in Propositions)
            {
                childCreated = TryToCreateAlphaRule(proposition);

                if (childCreated)
                {
                    return;
                }
            }

            foreach (Proposition proposition in Propositions)
            {
                childCreated = TryToCreateBetaRule(proposition);

                if (childCreated)
                {
                    return;
                }
            }
        }

        private bool TryToCreateDoubleNegation(Proposition proposition)
        {
            if (IsDoubleNegationRule(proposition))
            {
                ApplyDoubleNegationRule(proposition);

                return true;
            }

            return false;
        }

        private bool IsDoubleNegationRule(Proposition proposition)
        {
            if (proposition.GetType() == typeof(Negation))
            {
                Negation negation = (Negation)proposition;
                Proposition negatedProposition = negation.LeftSuccessor;

                if (negatedProposition.GetType() == typeof(Negation))
                {
                    return true;
                }
            }

            return false;
        }

        private void ApplyDoubleNegationRule(Proposition proposition)
        {
            HashSet<Proposition> propositions = new HashSet<Proposition>();

            foreach (Proposition p in Propositions)
            {
                if (!p.Equals(proposition))
                {
                    propositions.Add(p);
                }
            }

            Proposition doubleNegationRemoved = null;

            if (proposition.GetType() == typeof(Negation))
            {
                Negation negation = (Negation)proposition;
                Proposition negatedProposition = negation.LeftSuccessor;

                if (negatedProposition.GetType() == typeof(Negation))
                {
                    Negation negatedNegation = (Negation)negatedProposition;

                    doubleNegationRemoved = negatedNegation.LeftSuccessor;
                    propositions.Add(doubleNegationRemoved);
                }
            }

            LeftChild = new SemanticTableauxElement(propositions);
        }

        private bool TryToCreateBetaRule(Proposition proposition)
        {
            if (IsBetaRule(proposition))
            {
                ApplyBetaRule(proposition);

                return true;
            }

            return false;
        }

        private bool IsBetaRule(Proposition proposition)
        {
            if (proposition.GetType() == typeof(Negation))
            {
                Negation n = (Negation)proposition;

                return n.LeftSuccessor.GetType() == typeof(Conjunction)
                    || n.LeftSuccessor.GetType() == typeof(BiImplication);
            }

            if (proposition.GetType() == typeof(Nand))
            {
                return true;
            }

            if (proposition.GetType() == typeof(Disjunction))
            {
                return true;
            }

            if (proposition.GetType() == typeof(Implication))
            {
                return true;
            }

            if (proposition.GetType() == typeof(BiImplication))
            {
                return true;
            }

            return false;
        }

        private void ApplyBetaRule(Proposition proposition)
        {
            HashSet<Proposition> leftChildPropositions = new HashSet<Proposition>();
            HashSet<Proposition> rightChildPropositions = new HashSet<Proposition>();

            Proposition leftSetProposition = null;
            Proposition rightSetPropostion = null;

            foreach (Proposition p in Propositions)
            {
                if (!p.Equals(proposition))
                {
                    leftChildPropositions.Add(p);
                    rightChildPropositions.Add(p);
                }
            }

            BinaryConnective connective = null;

            if (proposition.GetType() == typeof(Negation) || proposition.GetType() == typeof(Nand))
            {
                Negation leftNegation = new Negation();
                Negation rightNegation = new Negation();

                // Possibly Not and or Not bi-implication
                if (proposition.GetType() == typeof(Negation))
                {
                    Negation negation = (Negation)proposition;
                    connective = (BinaryConnective)negation.LeftSuccessor;

                    // If Bi-Implication
                    if (connective.GetType() == typeof(BiImplication))
                    {
                        leftChildPropositions.Add(connective.LeftSuccessor);
                        leftNegation.LeftSuccessor = connective.RightSuccessor;

                        rightChildPropositions.Add(connective.RightSuccessor);
                        rightNegation.LeftSuccessor = connective.LeftSuccessor;
                    }
                    else
                    {
                        // If And
                        leftNegation.LeftSuccessor = connective.LeftSuccessor;
                        rightNegation.LeftSuccessor = connective.RightSuccessor;
                    }
                }
                else
                {
                    // If Nand
                    connective = (BinaryConnective)proposition;

                    leftNegation.LeftSuccessor = connective.LeftSuccessor;
                    rightNegation.LeftSuccessor = connective.RightSuccessor;
                }

                leftSetProposition = leftNegation;
                rightSetPropostion = rightNegation;
            }

            if (proposition.GetType() == typeof(Disjunction))
            {
                connective = (BinaryConnective)proposition;

                leftSetProposition = connective.LeftSuccessor;
                rightSetPropostion = connective.RightSuccessor;
            }

            if (proposition.GetType() == typeof(Implication))
            {
                Negation leftNegation = new Negation();
                connective = (BinaryConnective)proposition;

                leftNegation.LeftSuccessor = connective.LeftSuccessor;

                leftSetProposition = leftNegation;
                rightSetPropostion = connective.RightSuccessor;
            }

            if (proposition.GetType() == typeof(BiImplication))
            {
                connective = (BinaryConnective)proposition;

                Negation leftNegated = new Negation();
                leftNegated.LeftSuccessor = connective.LeftSuccessor;

                Negation rightNegated = new Negation();
                rightNegated.LeftSuccessor = connective.RightSuccessor;

                // First add the left part of the bi-implication to both left and right branch
                // sets
                leftChildPropositions.Add(connective.LeftSuccessor);
                rightChildPropositions.Add(leftNegated);

                // Then the right part to keep a certain order.
                leftSetProposition = connective.RightSuccessor;
                rightSetPropostion = rightNegated;
            }

            leftChildPropositions.Add(leftSetProposition);
            rightChildPropositions.Add(rightSetPropostion);

            LeftChild = new SemanticTableauxElement(leftChildPropositions);
            RightChild = new SemanticTableauxElement(rightChildPropositions);
        }

        private bool TryToCreateAlphaRule(Proposition proposition)
        {
            if (IsAlphaRule(proposition))
            {
                ApplyAlphaRule(proposition);
            
                return true;
            }

            return false;
        }

        private bool IsAlphaRule(Proposition proposition)
        {
            if (proposition.GetType() == typeof(Conjunction))
            {
                return true;
            }

            if (proposition.GetType() == typeof(Negation))
            {
                Negation n = (Negation)proposition;

                return n.LeftSuccessor.GetType() == typeof(Disjunction) || n.LeftSuccessor.GetType() == typeof(Implication);
            }

            return false;
        }

        private void ApplyAlphaRule(Proposition proposition)
        {
            HashSet<Proposition> childPropositions = new HashSet<Proposition>();

            foreach (Proposition p in Propositions)
            {
                if (!p.Equals(proposition))
                {
                    childPropositions.Add(p);
                }
            }

            if (proposition.GetType() == typeof(Conjunction))
            {
                Conjunction conjunction = (Conjunction)proposition;

                childPropositions.Add(conjunction.LeftSuccessor);
                childPropositions.Add(conjunction.RightSuccessor);
            }

            if (proposition.GetType() == typeof(Negation))
            {
                Negation negation = (Negation)proposition;
                BinaryConnective nestedConnective = (BinaryConnective)negation.LeftSuccessor;

                // Both cases have a negation of the right successor.
                Negation negatedRight = new Negation();
                negatedRight.LeftSuccessor = nestedConnective.RightSuccessor;

                // Only disjunction results in a left side negated as well.
                if (nestedConnective.GetType() == typeof(Disjunction))
                {
                    Negation negatedLeft = new Negation();
                    negatedLeft.LeftSuccessor = nestedConnective.LeftSuccessor;
                    
                    childPropositions.Add(negatedLeft);
                }

                // Should not forget to add the implication's left successor
                if (nestedConnective.GetType() == typeof(Implication))
                {
                    childPropositions.Add(nestedConnective.LeftSuccessor);
                }

                childPropositions.Add(negatedRight);
            }

            LeftChild = new SemanticTableauxElement(childPropositions);
        }

        [ExcludeFromCodeCoverage]
        public string NodeLabel()
        {
            string label = $"node{NodeNumber}[ label = \"";

            foreach(Proposition proposition in Propositions)
            {
                label += proposition + "\n";
            }

            label.Trim('\n');
            label += "\"";

            if (IsClosed() && LeftChild == null && RightChild == null)
            {
                label += ", color = red";
            }

            label += " ]\n";

            if (LeftChild != null)
            {
                label += $"node{NodeNumber} -- node{LeftChild.NodeNumber}\n";
                label += LeftChild.NodeLabel();
            }

            if (RightChild != null)
            {
                label += $"node{NodeNumber} -- node{RightChild.NodeNumber}\n";
                label += RightChild.NodeLabel();
            }

            return label;
        }
    }
}
