using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LogicAndSetTheoryApplication
{
    public class SemanticTableauxElement: IDotFile
    {
        private static string replacementVariableCharacters = "abcdefghijklmnopqrstuvw";
        private HashSet<Proposition> alreadyProcessedGammaRules;

        // Then if this is empty we won't display it
        public HashSet<char> ReplacementVariables { get; set; }

        public int NodeNumber { get; set; }
        public HashSet<Proposition> Propositions { get; }
        public SemanticTableauxElement LeftChild { get; private set; }
        public SemanticTableauxElement RightChild { get; private set; }

        public SemanticTableauxElement(HashSet<Proposition> propositions): this(propositions, new HashSet<char>())
        { }

        public SemanticTableauxElement(HashSet<Proposition> propositions, HashSet<char> replacementVariables)
        {
            if (propositions == null)
            {
                throw new NullReferenceException("Unexpected null argument, can not process tableaux element this way.");
            }

            alreadyProcessedGammaRules = new HashSet<Proposition>();
            ReplacementVariables = replacementVariables;
            Propositions = propositions;

            if (!IsClosed())
            {
                CreateChildren();
            }
        }

        public bool IsClosed()
        {
            List<Proposition> pSet = Propositions.ToList();

            // It can be closed if we have one constant False or one Negated True already.
            // Constants are only part of abstract propositions
            foreach (Proposition p in pSet)
            {
                if (ConstantContradiction(p))
                {
                    return true;
                }
            }

            // We could split it up by taking out this part and put it in a base class.
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

        // Specific to abstract propositions
        private bool ConstantContradiction(Proposition proposition)
        {
            if (proposition.GetType() == typeof(False))
            {
                return true;
            }

            if (proposition.GetType() == typeof(Negation))
            {
                Negation negation = (Negation)proposition;
                return negation.LeftSuccessor.GetType() == typeof(True);
            }

            return false;
        }

        // Both, but equals still needs to be overriden for predicates
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
                Proposition negatedSuccessor = negatedProposition.LeftSuccessor;
                return negatedSuccessor.Equals(targetLiteral);
            }

            return false;
        }

        // Both, just the ordering is different and for predicates we
        // have two additional rules that should be placed in between.
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
                childCreated = TryToCreateDeltaRule(proposition);

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

            HashSet<Proposition> childPropositionSet = new HashSet<Proposition>();

            foreach (Proposition proposition in Propositions)
            {
                if (IsGammaRule(proposition))
                {
                    foreach (Proposition p in ApplyGammaRule(proposition))
                    {
                        childPropositionSet.Add(p);
                    }
                }
            }

            if (childPropositionSet.Count > Propositions.Count)
            {
                LeftChild = new SemanticTableauxElement(childPropositionSet, ReplacementVariables);

                return;
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

            LeftChild = new SemanticTableauxElement(propositions, new HashSet<char>(ReplacementVariables));
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

                return n.LeftSuccessor.GetType() == typeof(Disjunction)
                    || n.LeftSuccessor.GetType() == typeof(Implication)
                    || n.LeftSuccessor.GetType() == typeof(Nand);
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
                BinaryConnective connective = (BinaryConnective)proposition;

                childPropositions.Add(connective.LeftSuccessor);
                childPropositions.Add(connective.RightSuccessor);
            }

            if (proposition.GetType() == typeof(Negation))
            {
                Negation negation = (Negation)proposition;
                BinaryConnective nestedConnective = (BinaryConnective)negation.LeftSuccessor;

                if (nestedConnective.GetType() != typeof(Nand))
                {
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
                else
                {
                    // It's a negated Nand and we can add left and right to the set
                    childPropositions.Add(nestedConnective.LeftSuccessor);
                    childPropositions.Add(nestedConnective.RightSuccessor);
                }

            }

            LeftChild = new SemanticTableauxElement(childPropositions, new HashSet<char>(ReplacementVariables));
        }

        private bool TryToCreateDeltaRule(Proposition proposition)
        {
            if (IsDeltaRule(proposition))
            {
                ApplyDeltaRule(proposition);

                return true;
            }

            return false;
        }

        private bool IsDeltaRule(Proposition proposition)
        {
            if (proposition.GetType() == typeof(ExistentialQuantifier))
            {
                return true;
            }
         
            // Negated Universal Quantifier
            if (proposition.GetType() == typeof(Negation))
            {
                Proposition left = ((Negation)proposition).LeftSuccessor;

                return left.GetType() == typeof(UniversalQuantifier);
            }

            return false;
        }

        private void ApplyDeltaRule(Proposition proposition)
        {
            // COULD BE: GetAllDifferingChildPropositions(proposition)
            // REFACTOR WORTHY: otherwise needs to be tested for every rule which is all the same
            HashSet<Proposition> childPropositions = new HashSet<Proposition>();

            foreach (Proposition p in Propositions)
            {
                if (!p.Equals(proposition))
                {
                    childPropositions.Add(p);
                }
            }
            // REFACTOR WORTHY

            // Current proposition we want to break up again into new pieces.
            Quantifier quantifier = null;
            Proposition predicate = null;

            if (proposition.GetType() == typeof(Negation))
            {
                Negation negatedUniversalQuantifier = (Negation)proposition;
                quantifier = (Quantifier)negatedUniversalQuantifier.LeftSuccessor;

                predicate = quantifier.LeftSuccessor;
                Negation negatedPredicate = new Negation();
                negatedPredicate.LeftSuccessor = predicate;

                predicate = negatedPredicate;
            }
            else
            {
                quantifier = (Quantifier)proposition;
                predicate = quantifier.LeftSuccessor;
            }

            char boundVariable = quantifier.GetBoundVariable();
            char replacementCharacter = GenerateReplacementVariable();

            Proposition predicateCopy = predicate.Copy();
            predicateCopy.Replace(boundVariable, replacementCharacter);

            // Get current set locally, add new variable and pass them down.
            HashSet<char> childReplacementVariables = new HashSet<char>(ReplacementVariables);
            childReplacementVariables.Add(replacementCharacter);

            childPropositions.Add(predicateCopy);

            LeftChild = new SemanticTableauxElement(childPropositions, childReplacementVariables);
        }

        private char GenerateReplacementVariable()
        {
            char replacementVariable;

            for (int i = 0; i < replacementVariableCharacters.Length; i++)
            {
                replacementVariable = replacementVariableCharacters[i];
                replacementVariableCharacters = replacementVariableCharacters.Substring(1);
                
                return replacementVariable;
            }

            throw new Exception("All variables exhausted, now what?");
        }

        public static void ResetReplacementVariables()
        {
            replacementVariableCharacters = "abcdefghijklmnopqrstuvw";
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
                    // Add copies to the set for all other propositions
                    // ensuring that each quantifier will at least be unique.
                    leftChildPropositions.Add(p.Copy());
                    rightChildPropositions.Add(p.Copy());
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

            LeftChild = new SemanticTableauxElement(leftChildPropositions, new HashSet<char>(ReplacementVariables));
            RightChild = new SemanticTableauxElement(rightChildPropositions, new HashSet<char>(ReplacementVariables));
        }

        private bool IsGammaRule(Proposition proposition)
        {
            if (proposition.GetType() == typeof(UniversalQuantifier))
            {
                return true;
            }

            if (proposition.GetType() == typeof(Negation))
            {
                Proposition quantifier = ((Negation)proposition).LeftSuccessor;

                return quantifier.GetType() == typeof(ExistentialQuantifier);
            }

            return false;
        }

        public HashSet<Proposition> ApplyGammaRule(Proposition proposition)
        {
            HashSet<Proposition> childPropositions = new HashSet<Proposition>();

            foreach (Proposition p in Propositions)
            {
                childPropositions.Add(p);
            }

            Quantifier quantifier = null;
            Proposition propositionToAdd = null;

            if (proposition.GetType() == typeof(Negation))
            {
                Negation negatedExistentialQuantifier = (Negation)proposition;
                quantifier = (Quantifier)negatedExistentialQuantifier.LeftSuccessor;

                propositionToAdd = quantifier.LeftSuccessor;
                Negation negatedPredicate = new Negation();
                negatedPredicate.LeftSuccessor = propositionToAdd;

                propositionToAdd = negatedPredicate;
            }
            else
            {
                quantifier = (Quantifier)proposition;
                propositionToAdd = quantifier.LeftSuccessor;
            }

            if (ReplacementVariables.Count > 0)
            {
                // But in this case we must most likely add a copy of the predicate
                // Because otherwise the quantifiers HashMap will be modified by reference.
                // Replace the variable that matches the bound var of quantifier.
                // Do this for each available replacement variable in the collection.
                char boundVariable = quantifier.GetBoundVariable();
                // For replacement we can just use the proposition class as the runtime binding will take care of replacing
                // it on the predicate.

                // Thus for anything except when we have a quantifier nested
                // we want to replace the bound variable for every available replacement
                // character I believe.
                // Replace the bound variable by every possible replacement variable.
                // And add that new predicate to the set.
                // Only if we are directly looking at a predicate
                foreach (char replacementChar in ReplacementVariables)
                {
                    // Only if replacement character was not yet applied to the quantifier.
                    if (quantifier.IsNotYetApplied(replacementChar))
                    {
                        Proposition propositionToAddCopy = propositionToAdd.Copy();
                        propositionToAddCopy.Replace(boundVariable, replacementChar);

                        childPropositions.Add(propositionToAddCopy);

                        quantifier.AddAppliedReplacementVariable(replacementChar);
                    }
                }
            }

            return childPropositions;
        }

        public string NodeLabel()
        {
            string label = $"node{NodeNumber}[ label = \"";

            if (ReplacementVariables.Count > 0)
            {
                label += "< ";
                
                foreach (char replacementVariable in ReplacementVariables)
                {
                    label += $"{replacementVariable}, ";
                }

                label = label.Substring(0, label.Length - 2);
                label += " >\n";
            }

            foreach(Proposition proposition in Propositions)
            {
                label += proposition + "\n";
            }

            label = label.Trim('\n');
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
