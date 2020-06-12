using System;
using System.Collections.Generic;
using System.Text;
using LogicAndSetTheoryApplication;
using Xunit;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using System.Linq;

namespace LPPUnitTests
{
    public class SemanticTableauxElementTests
    {
        [Fact]
        public void IsClosed_TestIsClosedWhenChildNullAndNoContradictionInSet_ExpectedFalseReturned()
        {
            // Arrange
            HashSet<Proposition> propositionSet = new HashSet<Proposition>()
            {
                PropositionGenerator.GetRandomProposition(),
                PropositionGenerator.GetRandomProposition()
            };

            SemanticTableauxElement alphaRuleElement = new SemanticTableauxElement(propositionSet);

            // Act
            bool isBranchClosed = alphaRuleElement.IsClosed();

            // Assert
            isBranchClosed.Should().BeFalse("Because the tableaux element has no contradiction and no children, thus was not closed.");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IsClosed_TestIsClosedWhenContradictionInSet_ExpectedTrueReturned(bool leftNegated)
        {
            // Arrange
            Disjunction fillerProposition = new Disjunction();
            Proposition child = new Proposition('Z');
            fillerProposition.LeftSuccessor = child;
            fillerProposition.RightSuccessor = child;

            Proposition p = new Proposition('G');
            Negation pNegated = new Negation();
            pNegated.LeftSuccessor = p;

            HashSet<Proposition> propositionSet = null;

            if (leftNegated)
            {
                propositionSet = new HashSet<Proposition>()
                {
                    pNegated,
                    fillerProposition,
                    fillerProposition,
                    p,
                    fillerProposition
                };
            }
            else
            {
                propositionSet = new HashSet<Proposition>()
                {
                    p,
                    fillerProposition,
                    fillerProposition,
                    pNegated,
                    fillerProposition
                };
            }

            SemanticTableauxElement alphaRuleElement = new SemanticTableauxElement(propositionSet);

            // Act
            bool isBranchClosed = alphaRuleElement.IsClosed();

            // Assert
            isBranchClosed.Should().BeTrue("Because the tableaux element has a contradiction in its set.");
        }

        [Fact]
        public void IsClosed_ConstantTrue_ExpectedFalseReturned()
        {
            // Arrange
            Proposition proposition = new True();

            HashSet<Proposition> propositionSet = new HashSet<Proposition>()
            {
                proposition
            };

            SemanticTableauxElement element = new SemanticTableauxElement(propositionSet);

            // Act
            bool isBranchClosed = element.IsClosed();

            // Assert
            isBranchClosed.Should().BeFalse("Because the tableaux element has true in its set, which does not contradict.");
        }

        [Fact]
        public void IsClosed_ContradictionByNegatedTrue_ExpectedTrueReturned()
        {
            // Arrange
            Negation negatedTrue = new Negation();
            Proposition proposition = new True();
            negatedTrue.LeftSuccessor = proposition;

            HashSet<Proposition> propositionSet = new HashSet<Proposition>()
            {
                negatedTrue
            };

            SemanticTableauxElement element = new SemanticTableauxElement(propositionSet);

            // Act
            bool isBranchClosed = element.IsClosed();

            // Assert
            isBranchClosed.Should().BeTrue("Because the tableaux element has a negated true in its set, which DOES contradict as it is equivalent to False.");
        }

        [Fact]
        public void IsClosed_ContradictionByConstantFalse_ExpectedTrueReturned()
        {
            // Arrange
            Proposition proposition = new False();

            HashSet<Proposition> propositionSet = new HashSet<Proposition>()
            {
                proposition
            };

            SemanticTableauxElement element = new SemanticTableauxElement(propositionSet);

            // Act
            bool isBranchClosed = element.IsClosed();

            // Assert
            isBranchClosed.Should().BeTrue("Because the tableaux element has false in its set which is a contradiction.");
        }

        [Fact]
        public void IsClosed_NegatedFalse_ExpectedFalseReturned()
        {
            // Arrange
            Negation negatedFalse = new Negation();
            Proposition proposition = new False();
            negatedFalse.LeftSuccessor = proposition;

            HashSet<Proposition> propositionSet = new HashSet<Proposition>()
            {
                negatedFalse
            };

            SemanticTableauxElement element = new SemanticTableauxElement(propositionSet);

            // Act
            bool isBranchClosed = element.IsClosed();

            // Assert
            isBranchClosed.Should().BeFalse("Because the tableaux element has a negated false (aka true) in its set.");
        }

        [Fact]
        public void IsClosed_ContradictionByConjunction_AfterCreatingChildElementContainsContradictionClosedShouldReturnTrue()
        {
            // Arrange
            Conjunction contradiction = new Conjunction();
            
            Proposition child = new Proposition('Z');
            contradiction.LeftSuccessor = child;
            
            Negation negatedChild = new Negation();
            negatedChild.LeftSuccessor = child;
            contradiction.RightSuccessor = negatedChild;

            Proposition p = new Proposition('G');

            HashSet<Proposition> propositionSet = new HashSet<Proposition>()
            {
                contradiction,
                p
            };

            // Act
            SemanticTableauxElement alphaRuleElement = new SemanticTableauxElement(propositionSet);
            bool alphaRuleClosedByChild = alphaRuleElement.IsClosed();

            // Assert
            alphaRuleClosedByChild.Should().BeTrue("Because the child alpha rule element should close the branch by contradiction.");
        }


        [Fact]
        public void IsClosed_NoContradictionInSetWithConjunctionAsAlphaRuleChild_AfterCreatingChildContainsIsClosedShouldReturnFalse()
        {
            // Arrange
            Conjunction conjunction = new Conjunction();
            Proposition child = new Proposition('Z');
            conjunction.LeftSuccessor = child;
            conjunction.RightSuccessor = child;

            HashSet<Proposition> propositionSet = new HashSet<Proposition>()
            {
                conjunction
            };

            // Act
            SemanticTableauxElement alphaRuleElement = new SemanticTableauxElement(propositionSet);
            bool alphaRuleClosedByChild = alphaRuleElement.IsClosed();

            // Assert
            alphaRuleClosedByChild.Should().BeFalse("Because the child alpha rule element does not contain a contradiction.");
        }

        [Fact]
        public void Constructor_NullGivenAsPropositionSet_ExpectedNullReferenceExceptionThrown()
        {
            // Arrange
            Action act = () => new SemanticTableauxElement(null);

            // Act // Assert
            act.Should().Throw<NullReferenceException>("Because it is not allowed to pass null for the proposition set on which the tableaux element relies.");
        }

        [Fact]
        public void Constructor_AlphaRuleGiven_ExpectedLeftChildToNotBeNullAndASemanticTableauxElement()
        {
            // Arrange
            Conjunction conjunction = (Conjunction)PropositionGenerator.CreateBinaryConnectiveWithRandomSymbols(Conjunction.SYMBOL);

            HashSet<Proposition> propositions = new HashSet<Proposition>()
            {
                conjunction
            };

            // Act
            SemanticTableauxElement semanticTableauxElement = new SemanticTableauxElement(propositions);

            // Assert
            string message = "Because on an alpha rule only one child is created";

            semanticTableauxElement.LeftChild.Should().BeOfType<SemanticTableauxElement>(message);
            semanticTableauxElement.RightChild.Should().BeNull(message);
        }

        [Fact]
        public void Constructor_BetaRuleGiven_ExpectedLeftAndRightChildToNotBeNullAndASemanticTableauxElement()
        {
            // Arrange
            Disjunction disjunction = (Disjunction)PropositionGenerator.CreateBinaryConnectiveWithRandomSymbols(Disjunction.SYMBOL);

            HashSet<Proposition> propositions = new HashSet<Proposition>()
            {
                disjunction
            };

            // Act
            SemanticTableauxElement semanticTableauxElement = new SemanticTableauxElement(propositions);

            // Assert
            string message = "Because on a beta rule both children are created";

            semanticTableauxElement.LeftChild.Should().BeOfType<SemanticTableauxElement>(message);
            semanticTableauxElement.RightChild.Should().BeOfType<SemanticTableauxElement>(message);
        }

        [Fact]
        public void Constructor_CreateAlphaRuleFromConjunctionOfLiterals_SetOfLengthTwoReturnedWithTwoLiterals()
        {
            // Arrange
            Conjunction conjunction = (Conjunction) PropositionGenerator.CreateBinaryConnectiveWithRandomSymbols(Conjunction.SYMBOL);
            HashSet<Proposition> propositions = new HashSet<Proposition>()
            {
                conjunction
            };

            SemanticTableauxElement alphaRuleElement = new SemanticTableauxElement(propositions);
            SemanticTableauxElement child = alphaRuleElement.LeftChild;

            // Act
            HashSet<Proposition> childPropositions = child.Propositions;

            int actualNumberOfLiterals = childPropositions.Count;
            int expectedNumberOfLiterals = 2;

            // Assert
            actualNumberOfLiterals.Should().Be(expectedNumberOfLiterals, "Because a conjunction of two literals was given");
        }

        [Fact]
        public void Constructor_CreateAlphaRuleFromNegatedNandOfLiterals_SetOfLengthTwoReturnedWithTwoLiterals()
        {
            // Arrange
            Nand nand = (Nand)PropositionGenerator.CreateBinaryConnectiveWithRandomSymbols(Nand.SYMBOL);
            Negation negatedNand = new Negation();
            negatedNand.LeftSuccessor = nand;

            HashSet<Proposition> propositions = new HashSet<Proposition>()
            {
                negatedNand
            };

            SemanticTableauxElement alphaRuleElement = new SemanticTableauxElement(propositions);
            SemanticTableauxElement child = alphaRuleElement.LeftChild;

            // Act
            HashSet<Proposition> childPropositions = child.Propositions;

            int actualNumberOfLiterals = childPropositions.Count;
            int expectedNumberOfLiterals = 2;

            // Assert
            actualNumberOfLiterals.Should().Be(expectedNumberOfLiterals, "Because a conjunction of two literals was given");
        }

        [Fact]
        public void Constructor_CreateAlphaRuleFromNegationOfDisjunctionOfLiterals_SetOfLengthTwoReturnedWithTwoNegatedLiterals()
        {
            // Arrange
            Disjunction disjunction = (Disjunction)PropositionGenerator.CreateBinaryConnectiveWithRandomSymbols(Disjunction.SYMBOL);
            
            Negation negatedDisjunction = new Negation();
            negatedDisjunction.LeftSuccessor = disjunction;

            HashSet<Proposition> propositions = new HashSet<Proposition>()
            {
                negatedDisjunction
            };

            SemanticTableauxElement alphaRuleElement = new SemanticTableauxElement(propositions);
            SemanticTableauxElement child = alphaRuleElement.LeftChild;

            // Act
            HashSet<Proposition> childPropositions = child.Propositions;

            int actualNumberOfLiterals = childPropositions.Count;
            int expectedNumberOfLiterals = 2;

            // Assert
            actualNumberOfLiterals.Should().Be(expectedNumberOfLiterals, "Because a disjunction of two literals was given");

            foreach (Proposition p in childPropositions)
            {
                p.Should().BeOfType<Negation>("Because for this alpha rule, the negation of both literals is returned");
            }
        }

        [Fact]
        public void Constructor_CreateAlphaRuleFromNegationOfImplicationOfLiterals_SetOfLengthTwoReturnedWithRightSuccessorNegated()
        {
            // Arrange
            Implication implication = (Implication)PropositionGenerator.CreateBinaryConnectiveWithRandomSymbols(Implication.SYMBOL);

            Negation negatedImplication = new Negation();
            negatedImplication.LeftSuccessor = implication;

            HashSet<Proposition> propositions = new HashSet<Proposition>()
            {
                negatedImplication
            };

            SemanticTableauxElement alphaRuleElement = new SemanticTableauxElement(propositions);
            SemanticTableauxElement child = alphaRuleElement.LeftChild;

            // Act
            HashSet<Proposition> childPropositions = child.Propositions;

            int actualNumberOfLiterals = childPropositions.Count;
            int expectedNumberOfLiterals = 2;

            // Assert
            actualNumberOfLiterals.Should().Be(expectedNumberOfLiterals, "Because an implication of two literals was given");
            
            foreach(Proposition proposition in childPropositions)
            {
                if (proposition.GetType() == typeof(Negation))
                {
                    Negation negatedLiteral = (Negation)proposition;
                    negatedLiteral.LeftSuccessor.CompareTo(implication.RightSuccessor).Should().Be(0, "Because it is right negated successor of the implication.");
                }
                else
                {
                    proposition.CompareTo(implication.LeftSuccessor).Should().Be(0, "Because it is the left not negated successor of the implication.");
                }
            }
        }

        [Fact]
        public void Constructor_CreateBetaRuleFromNegationOfConjunctionOfLiterals_SetOfLengthOneForEachChildWithNegatedLiterals()
        {
            // Arrange
            Conjunction conjunction = (Conjunction)PropositionGenerator.CreateBinaryConnectiveWithRandomSymbols(Conjunction.SYMBOL);
            Negation negatedConjunction = new Negation();
            negatedConjunction.LeftSuccessor = conjunction;

            // Act // Assert
            TestConstructorForNotAndEquivalent(negatedConjunction);
        }

        private void TestConstructorForNotAndEquivalent(UnaryConnective connective)
        {
            // Arrange
            HashSet<Proposition> propositions = new HashSet<Proposition>()
            {
                connective
            };

            SemanticTableauxElement betaRuleElement = new SemanticTableauxElement(propositions);
            SemanticTableauxElement leftChild = betaRuleElement.LeftChild;
            SemanticTableauxElement rightChild = betaRuleElement.RightChild;

            // Act
            HashSet<Proposition> leftChildPropositions = leftChild.Propositions;
            HashSet<Proposition> rightChildPropositions = rightChild.Propositions;

            int actualNumberOfLiteralsInLeftChild = leftChildPropositions.Count;
            int actualNumberOfLiteralsInRightChild = rightChildPropositions.Count;

            int expectedNumberOfLiterals = 1;

            // Assert
            string message = "Because both proposition literals have been separated into two different sets";

            actualNumberOfLiteralsInLeftChild.Should().Be(expectedNumberOfLiterals, message);
            actualNumberOfLiteralsInRightChild.Should().Be(expectedNumberOfLiterals, message);

            message = "Because based on the rule, both children should be negated.";
            foreach (Proposition proposition in leftChildPropositions)
            {
                proposition.Should().BeOfType<Negation>(message);
            }

            foreach (Proposition proposition in rightChildPropositions)
            {
                proposition.Should().BeOfType<Negation>(message);
            }
        }

        [Fact]
        public void Constructor_CreateBetaRuleFromNandOfLiterals_SetOfLengthOneForEachChildWithNegatedLiterals()
        {
            // Arrange
            Nand nand = (Nand)PropositionGenerator.CreateBinaryConnectiveWithRandomSymbols(Nand.SYMBOL);

            TestConstructorForNotAndEquivalent(nand);
        }

        [Fact]
        public void Constructor_CreateBetaRuleFromDisjunctionOfLiterals_SetOfLengthOneForEachChildWithRegularLiterals()
        {
            // Arrange
            Disjunction disjunction = (Disjunction)PropositionGenerator.CreateBinaryConnectiveWithRandomSymbols(Disjunction.SYMBOL);

            HashSet<Proposition> propositions = new HashSet<Proposition>()
            {
                disjunction
            };

            SemanticTableauxElement betaRuleElement = new SemanticTableauxElement(propositions);
            SemanticTableauxElement leftChild = betaRuleElement.LeftChild;
            SemanticTableauxElement rightChild = betaRuleElement.RightChild;

            // Act
            HashSet<Proposition> leftChildPropositions = leftChild.Propositions;
            HashSet<Proposition> rightChildPropositions = rightChild.Propositions;

            int actualNumberOfLiteralsInLeftChild = leftChildPropositions.Count;
            int actualNumberOfLiteralsInRightChild = rightChildPropositions.Count;

            int expectedNumberOfLiterals = 1;

            // Assert
            string message = "Because both proposition literals have been separated into two different sets";

            actualNumberOfLiteralsInLeftChild.Should().Be(expectedNumberOfLiterals, message);
            actualNumberOfLiteralsInRightChild.Should().Be(expectedNumberOfLiterals, message);

            message = "Because based on the rule, both children should be separated into two different sets respectively.";
            foreach (Proposition proposition in leftChildPropositions)
            {
                proposition.Should().Be(disjunction.LeftSuccessor, message);
            }

            foreach (Proposition proposition in rightChildPropositions)
            {
                proposition.Should().Be(disjunction.RightSuccessor, message);
            }
        }

        [Fact]
        public void Constructor_CreateBetaRuleFromImplicationOfLiterals_SetOfLengthOneForEachChildWithOneNegatedLiteral()
        {
            // Arrange
            Implication implication = (Implication)PropositionGenerator.CreateBinaryConnectiveWithRandomSymbols(Implication.SYMBOL);

            HashSet<Proposition> propositions = new HashSet<Proposition>()
            {
                implication
            };

            SemanticTableauxElement betaRuleElement = new SemanticTableauxElement(propositions);
            SemanticTableauxElement leftChild = betaRuleElement.LeftChild;
            SemanticTableauxElement rightChild = betaRuleElement.RightChild;

            // Act
            HashSet<Proposition> leftChildPropositions = leftChild.Propositions;
            HashSet<Proposition> rightChildPropositions = rightChild.Propositions;

            int actualNumberOfLiteralsInLeftChild = leftChildPropositions.Count;
            int actualNumberOfLiteralsInRightChild = rightChildPropositions.Count;

            int expectedNumberOfLiterals = 1;

            // Assert
            string message = "Because both proposition literals have been separated into two different sets";

            actualNumberOfLiteralsInLeftChild.Should().Be(expectedNumberOfLiterals, message);
            actualNumberOfLiteralsInRightChild.Should().Be(expectedNumberOfLiterals, message);

            message = "Because based on the rule, the left child should be negated only.";
            foreach (Proposition proposition in leftChildPropositions)
            {
                proposition.Should().BeOfType<Negation>(message);
            }

            foreach (Proposition proposition in rightChildPropositions)
            {
                proposition.Should().Be(implication.RightSuccessor, message);
            }
        }

        [Fact]
        public void Constructor_CreateAlphaRuleFromConjunctionOfLiteralsWhenBetaRuleInPropositionSetAsWell_ChildrenShouldBeCreatedBasedOnAlphaRule()
        {
            // Arrange
            Conjunction conjunction = (Conjunction)PropositionGenerator.CreateBinaryConnectiveWithRandomSymbols(Conjunction.SYMBOL);
            Disjunction disjunction = (Disjunction)PropositionGenerator.CreateBinaryConnectiveWithRandomSymbols(Disjunction.SYMBOL);

            HashSet<Proposition> propositions = new HashSet<Proposition>()
            {
                disjunction,
                conjunction
            };

            // Act
            SemanticTableauxElement betaRuleElement = new SemanticTableauxElement(propositions);
            SemanticTableauxElement leftChild = betaRuleElement.LeftChild;
            SemanticTableauxElement rightChild = betaRuleElement.RightChild;

            // Assert
            leftChild.Should().BeOfType<SemanticTableauxElement>("Because the alpha rule should be applied");
            rightChild.Should().BeNull("Because when applying the alpha rule the right child would not be assigned");
        }

        [Fact]
        public void IsClosed_CreateBetaRuleFromDisjunctionWithAdditionalLiteralContradictingRightBranch_ExpectedRightBranchClosedButNotTheParent()
        {
            // Arrange
            Disjunction disjunction = (Disjunction)PropositionGenerator.CreateBinaryConnectiveWithRandomSymbols(Disjunction.SYMBOL);

            Negation negatedRightLiteral = new Negation();
            negatedRightLiteral.LeftSuccessor = disjunction.RightSuccessor;

            HashSet<Proposition> propositions = new HashSet<Proposition>()
            {
                disjunction,
                negatedRightLiteral
            };

            // Act
            SemanticTableauxElement betaRuleElement = new SemanticTableauxElement(propositions);
            bool parentClosed = betaRuleElement.IsClosed();
            bool rightClosed = betaRuleElement.RightChild.IsClosed();

            // Assert
            parentClosed.Should().BeFalse("Because only the right branch is closed off by a contradiction.");
            rightClosed.Should().BeTrue("Because the beta rule contains a contradiction for the right branch");
        }

        [Fact]
        public void IsClosed_CreateBetaRuleFromDisjunctionWithAdditionalLiteralsContradictingBothBranches_ExpectedParentClosed()
        {
            // Arrange
            Disjunction disjunction = (Disjunction)PropositionGenerator.CreateBinaryConnectiveWithRandomSymbols(Disjunction.SYMBOL);

            Negation negatedLeftLiteral = new Negation();
            negatedLeftLiteral.LeftSuccessor = disjunction.LeftSuccessor;

            Negation negatedRightLiteral = new Negation();
            negatedRightLiteral.LeftSuccessor = disjunction.RightSuccessor;

            HashSet<Proposition> propositions = new HashSet<Proposition>()
            {
                negatedLeftLiteral,
                disjunction,
                negatedRightLiteral
            };

            // Act
            SemanticTableauxElement betaRuleElement = new SemanticTableauxElement(propositions);
            bool parentClosed = betaRuleElement.IsClosed();

            // Assert
            parentClosed.Should().BeTrue("Because the beta rule contains two contradictions, each of which should close one of the branches");
        }

        [Fact]
        public void Constructor_CreateDoubleNegationOfLiteral_LeftChildCreatedWithNegationRemoved()
        {
            // Arrange
            Negation negation = (Negation)PropositionGenerator.CreateUnaryConnectiveWithRandomSymbol(Negation.SYMBOL);
            Negation doubleNegation = new Negation();
            doubleNegation.LeftSuccessor = negation;

            HashSet<Proposition> propositions = new HashSet<Proposition>()
            {
                doubleNegation
            };

            // Act
            SemanticTableauxElement semanticTableauxElement = new SemanticTableauxElement(propositions);
            HashSet<Proposition> propositionSetOfChild = semanticTableauxElement.LeftChild.Propositions;

            int actualNumberOfPropositionsInSet = propositionSetOfChild.Count;
            int expectedNumberOfPropositionsInSet = 1;

            Proposition expectedPropositionInSet = negation.LeftSuccessor;

            // Assert
            actualNumberOfPropositionsInSet.Should().Be(expectedNumberOfPropositionsInSet, "Because after removing the negation of one proposition we are left with one proposition");
            
            foreach(Proposition p in propositionSetOfChild)
            {
                p.Should().Be(expectedPropositionInSet, "Because the inner most child will be left over after removing it's encapsulating negations");
            }
        }

        [Fact]
        public void Constructor_CreateDoubleNegationOfLiteralWithAlphaRuleInSet_DoubleNegationShouldHavePrecedence()
        {
            // Arrange
            Negation negation = (Negation)PropositionGenerator.CreateUnaryConnectiveWithRandomSymbol(Negation.SYMBOL);
            Negation doubleNegation = new Negation();
            doubleNegation.LeftSuccessor = negation;

            Conjunction conjunction = (Conjunction)PropositionGenerator.CreateBinaryConnectiveWithRandomSymbols(Conjunction.SYMBOL);

            HashSet<Proposition> propositions = new HashSet<Proposition>()
            {
                conjunction,
                doubleNegation
            };

            // Act
            SemanticTableauxElement semanticTableauxElement = new SemanticTableauxElement(propositions);
            HashSet<Proposition> propositionSetOfChild = semanticTableauxElement.LeftChild.Propositions;

            int actualNumberOfPropositionsInSet = propositionSetOfChild.Count;
            int expectedNumberOfPropositionsInSet = 2;

            // Assert
            semanticTableauxElement.RightChild.Should().BeNull("Because the double negation is applied first, and thus only the left child is assigned");
            actualNumberOfPropositionsInSet.Should().Be(expectedNumberOfPropositionsInSet, "Because double negation would be removed first, and thus the number of propositions in the set of the first child should be " + expectedNumberOfPropositionsInSet);
        }

        // Bi-Implication rules
        [Fact]
        public void Constructor_CreateBiImplicationOfLiterals_BetaRuleShouldBeAppliedBothChildrenShouldHaveSetsOfTwoElementsOneLiteralsOneNegatedLiterals()
        {
            // Arrange
            BiImplication biImplication = (BiImplication)PropositionGenerator.CreateBinaryConnectiveWithRandomSymbols(BiImplication.SYMBOL);

            HashSet<Proposition> propositions = new HashSet<Proposition>()
            {
                biImplication
            };

            // Act
            SemanticTableauxElement semanticTableauxElement = new SemanticTableauxElement(propositions);
            HashSet<Proposition> propositionSetOfLeftChild = semanticTableauxElement.LeftChild.Propositions;
            HashSet<Proposition> propositionSetOfRightChild = semanticTableauxElement.RightChild.Propositions;

            int actualNumberOfPropositionsInLeftSet = propositionSetOfLeftChild.Count;
            int actualNumberOfPropositionsInRightSet = propositionSetOfRightChild.Count;
            int expectedNumberOfPropositionsInSet = 2;

            // Assert
            string message = "Because both the left and right set should have two children in their set after applying beta rule for bi-implication.";
            actualNumberOfPropositionsInLeftSet.Should().Be(expectedNumberOfPropositionsInSet, message);
            actualNumberOfPropositionsInRightSet.Should().Be(expectedNumberOfPropositionsInSet, message);

            TestSetToHaveOnlyLiteralsOrNegatedLiterals(propositionSetOfLeftChild);
            TestSetToHaveOnlyLiteralsOrNegatedLiterals(propositionSetOfRightChild);
        }

        private void TestSetToHaveOnlyLiteralsOrNegatedLiterals(HashSet<Proposition> propositions)
        {
            string message = "Because this set should contain both the ";

            string messageIfNotNegated = message + "regular literals";
            string messageIfNegated = message + "negated literals";

            bool testForNegations = false;
            foreach (Proposition p in propositions)
            {
                if (p.GetType() == typeof(Negation))
                {
                    testForNegations = true;
                }

                break;
            }

            // Test all the propositions to be of the same type now.
            foreach (Proposition p in propositions)
            {
                if (testForNegations)
                {
                    p.Should().BeOfType<Negation>(messageIfNegated);
                }
                else
                {
                    p.Should().BeOfType<Proposition>(messageIfNotNegated);
                }
            }
        }

        [Fact]
        public void IsClosed_CreateBiImplicationOfLiteralsWithLiteralsInParentSetToCloseBranches_ParentElementShouldBeClosed()
        {
            // Arrange
            BiImplication biImplication = (BiImplication)PropositionGenerator.CreateBinaryConnectiveWithRandomSymbols(BiImplication.SYMBOL);
            Proposition literal = biImplication.LeftSuccessor;
            Negation negatedLiteral = new Negation();
            negatedLiteral.LeftSuccessor = biImplication.LeftSuccessor;

            HashSet<Proposition> propositions = new HashSet<Proposition>()
            {
                literal,
                biImplication,
                negatedLiteral
            };

            // Act
            SemanticTableauxElement semanticTableauxElement = new SemanticTableauxElement(propositions);

            // Assert
            semanticTableauxElement.IsClosed().Should().BeTrue("Because it should be possible to close branches when there is at least any of the literals in the parent set as well as any negated literal");
        }

        [Fact]
        public void Constructor_CreateNegatedBiImplicationOfLiterals_BetaRuleShouldBeAppliedBothChildrenShouldHaveSetsOfTwoElementsOneLiteralAndOneNegatedLiteralInEachSet()
        {
            // Arrange
            Negation negatedBiImplication = new Negation();
            BiImplication biImplication = (BiImplication)PropositionGenerator.CreateBinaryConnectiveWithRandomSymbols(BiImplication.SYMBOL);
            negatedBiImplication.LeftSuccessor = biImplication;

            HashSet<Proposition> propositions = new HashSet<Proposition>()
            {
                negatedBiImplication
            };

            // Act
            SemanticTableauxElement semanticTableauxElement = new SemanticTableauxElement(propositions);
            HashSet<Proposition> propositionSetOfLeftChild = semanticTableauxElement.LeftChild.Propositions;
            HashSet<Proposition> propositionSetOfRightChild = semanticTableauxElement.RightChild.Propositions;

            int actualNumberOfPropositionsInLeftSet = propositionSetOfLeftChild.Count;
            int actualNumberOfPropositionsInRightSet = propositionSetOfRightChild.Count;
            int expectedNumberOfPropositionsInSet = 2;

            // Assert
            string message = "Because both the left and right set should have two children in their set after applying beta rule for negated bi-implication.";
            actualNumberOfPropositionsInLeftSet.Should().Be(expectedNumberOfPropositionsInSet, message);
            actualNumberOfPropositionsInRightSet.Should().Be(expectedNumberOfPropositionsInSet, message);

            TestSetToHaveOneLiteralAndOneNegatedLiteral(propositionSetOfLeftChild);
            TestSetToHaveOneLiteralAndOneNegatedLiteral(propositionSetOfRightChild);
        }

        private void TestSetToHaveOneLiteralAndOneNegatedLiteral(HashSet<Proposition> propositions)
        {
            bool negatedChecked = false;
            bool literalChecked = false;

            foreach(Proposition p in propositions)
            {
                if (!negatedChecked && !literalChecked)
                {
                    if (p.GetType() == typeof(Negation))
                    {
                        p.Should().BeOfType<Negation>("Because we are looking at a negation");
                    }
                    else
                    {
                        p.Should().BeOfType<Proposition>("Because we are looking at a literal");
                    }
                }
                else
                {
                    if (negatedChecked)
                    {
                        p.Should().BeOfType<Proposition>("Because the left over literal should be a proposition now");
                    }
                    else
                    {
                        p.Should().BeOfType<Negation>("Because the left over literal should be a negation now");
                    }
                }

            }
        }

        [Fact]
        public void IsClosed_CreateNegatedBiImplicationOfLiteralsWithLiteralsInParentSetToCloseBranches_ParentElementShouldBeClosed()
        {
            // Arrange
            BiImplication biImplication = (BiImplication)PropositionGenerator.CreateBinaryConnectiveWithRandomSymbols(BiImplication.SYMBOL);
            Proposition literal = biImplication.LeftSuccessor;
            Negation negatedLiteral = new Negation();
            negatedLiteral.LeftSuccessor = biImplication.LeftSuccessor;

            HashSet<Proposition> propositions = new HashSet<Proposition>()
            {
                literal,
                biImplication,
                negatedLiteral
            };

            // Act
            SemanticTableauxElement semanticTableauxElement = new SemanticTableauxElement(propositions);

            // Assert
            semanticTableauxElement.IsClosed().Should().BeTrue("Because it should be possible to close branches when there is at least any of the literals in the parent set as well as any negated literal");
        }

        // Create another construction test that tests if only the left has been set and right is null
        // after applying a gamma or delta rule.
        // And test the number of elements in the set.

        // The predicate's variable should be replaced
        // The tableaux element should have the used variable stored in it's set/list of used variables.

        // Another thing could be that the tableaux element needs to pass a replaced variable down each level.
        [Fact]
        public void Constructor_CreateNegatedUniversalQuantifier_DeltaRuleShouldBeAppliedChildShouldBePredicateAndVariableShouldBeIntroduced()
        {
            // Arrange
            char boundVariable = PropositionGenerator.GenerateBoundVariable();
            
            List<char> boundVariables = new List<char>() { boundVariable };
            Predicate predicate = new Predicate(PropositionGenerator.GetRandomVariableLetter(), boundVariables);
            
            UniversalQuantifier universalQuantifier = new UniversalQuantifier(boundVariable);
            universalQuantifier.LeftSuccessor = predicate;

            Negation negatedUniversalQuantifier = new Negation();
            negatedUniversalQuantifier.LeftSuccessor = universalQuantifier;
            
            HashSet<Proposition> propositions = new HashSet<Proposition>()
            {
                negatedUniversalQuantifier
            };

            // Act
            SemanticTableauxElement semanticTableauxElement = new SemanticTableauxElement(propositions);
            List<char> replacementVariables = semanticTableauxElement.ReplacementVariables.ToList();

            int expectedNumberOfReplacementVariables = 1;
            int actualNumberOfReplacementVariables = replacementVariables.Count;

            // Assert
            actualNumberOfReplacementVariables.Should().Be(expectedNumberOfReplacementVariables, "Because the only bound variable should be replaced based on the rules");

            foreach(Proposition proposition in semanticTableauxElement.LeftChild.Propositions)
            {
                if (proposition is Predicate)
                {
                    predicate = (Predicate)proposition;
                    bool isReplaced = predicate.IsReplaced(boundVariable);
                    isReplaced.Should().BeTrue("Because after applying a delta rule, the only bound variable in the predicate should be replaced");
                }
            }
        }

        [Fact]
        public void Constructor_CreateExistentialQuantifier_DeltaRuleShouldBeAppliedChildShouldBePredicateAndVariableShouldBeIntroduced()
        {
            // Arrange
            char boundVariable = PropositionGenerator.GenerateBoundVariable();

            List<char> boundVariables = new List<char>() { boundVariable };
            Predicate predicate = new Predicate(PropositionGenerator.GetRandomVariableLetter(), boundVariables);

            ExistentialQuantifier existentialQuantifier = new ExistentialQuantifier(boundVariable);
            existentialQuantifier.LeftSuccessor = predicate;

            HashSet<Proposition> propositions = new HashSet<Proposition>()
            {
                existentialQuantifier
            };

            // Act
            SemanticTableauxElement semanticTableauxElement = new SemanticTableauxElement(propositions);
            List<char> replacementVariables = semanticTableauxElement.ReplacementVariables.ToList();

            int expectedNumberOfReplacementVariables = 1;
            int actualNumberOfReplacementVariables = replacementVariables.Count;

            // Assert
            actualNumberOfReplacementVariables.Should().Be(expectedNumberOfReplacementVariables, "Because the only bound variable should be replaced based on the rules");

            foreach (Proposition proposition in semanticTableauxElement.LeftChild.Propositions)
            {
                if (proposition is Predicate)
                {
                    predicate = (Predicate)proposition;
                    bool isReplaced = predicate.IsReplaced(boundVariable);
                    isReplaced.Should().BeTrue("Because after applying a delta rule, the only bound variable in the predicate should be replaced");
                }
            }
        }

        [Fact]
        public void Constructor_CreateNegatedExistentialQuantifierNoReplacementVariablesPresent_GammaRuleShouldNotBeAppliedNoChildrenGenerated()
        {
            // Arrange
            char boundVariable = PropositionGenerator.GenerateBoundVariable();

            List<char> boundVariables = new List<char>() { boundVariable };
            Predicate predicate = new Predicate(PropositionGenerator.GetRandomVariableLetter(), boundVariables);

            ExistentialQuantifier existentialQuantifier = new ExistentialQuantifier(boundVariable);
            existentialQuantifier.LeftSuccessor = predicate;

            Negation negatedExistentialQuantifier = new Negation();
            negatedExistentialQuantifier.LeftSuccessor = existentialQuantifier;

            HashSet<Proposition> propositions = new HashSet<Proposition>()
            {
                negatedExistentialQuantifier
            };

            // Act
            SemanticTableauxElement semanticTableauxElement = new SemanticTableauxElement(propositions);
            bool isReplaced = predicate.IsReplaced(boundVariable);

            // Assert
            isReplaced.Should().BeFalse("Because no replacement variables are available");
            semanticTableauxElement.LeftChild.Should().BeNull("Because no replacement variables are available and thus no child was created");
        }

        [Fact]
        public void Constructor_CreateUniversalQuantifierNoReplacementVariablesPresent_GammaRuleShouldNotBeAppliedNoChildrenGenerated()
        {
            // Arrange
            char boundVariable = PropositionGenerator.GenerateBoundVariable();

            List<char> boundVariables = new List<char>() { boundVariable };
            Predicate predicate = new Predicate(PropositionGenerator.GetRandomVariableLetter(), boundVariables);

            UniversalQuantifier universalQuantifier = new UniversalQuantifier(boundVariable);
            universalQuantifier.LeftSuccessor = predicate;

            HashSet<Proposition> propositions = new HashSet<Proposition>()
            {
                universalQuantifier
            };

            // Act
            SemanticTableauxElement semanticTableauxElement = new SemanticTableauxElement(propositions);
            bool isReplaced = predicate.IsReplaced(boundVariable);

            // Assert
            isReplaced.Should().BeFalse("Because no replacement variables are available");
            semanticTableauxElement.LeftChild.Should().BeNull("Because no replacement variables are available and thus no child was created");
        }

        [Fact]
        public void Constructor_CreateUniversalQuantifierWithReplacementVariablePresent_GammaRuleShouldBeAppliedAndNewChildrenCreated()
        {
            // Arrange
            char boundVariable = PropositionGenerator.GenerateBoundVariable();

            List<char> boundVariables = new List<char>() { boundVariable };
            Predicate predicate = new Predicate(PropositionGenerator.GetRandomVariableLetter(), boundVariables);

            UniversalQuantifier universalQuantifier = new UniversalQuantifier(boundVariable);
            universalQuantifier.LeftSuccessor = predicate;

            HashSet<Proposition> propositions = new HashSet<Proposition>()
            {
                universalQuantifier
            };

            // Act
            char availableReplacementVariable = 'd';
            SemanticTableauxElement semanticTableauxElement = new SemanticTableauxElement(propositions, new HashSet<char>() { availableReplacementVariable }) ;

            // Assert
            semanticTableauxElement.LeftChild.Should().NotBeNull("Because children should be created now that a replacement variable is present");
            
            foreach(Proposition proposition in semanticTableauxElement.LeftChild.Propositions)
            {
                if (proposition is Predicate)
                {
                    Predicate pred = (Predicate)proposition;
                    bool isReplaced = pred.IsReplaced(boundVariable);
                    isReplaced.Should().BeTrue($"Because the replacement variable {availableReplacementVariable} is available");
                }
            }
        }

        [Fact]
        public void Constructor_CreateNegatedExistentialQuantifierWithReplacementVariablePresent_GammaRuleShouldBeAppliedAndNewChildrenCreated()
        {
            // Arrange
            char boundVariable = PropositionGenerator.GenerateBoundVariable();

            List<char> boundVariables = new List<char>() { boundVariable };
            Predicate predicate = new Predicate(PropositionGenerator.GetRandomVariableLetter(), boundVariables);

            ExistentialQuantifier existentialQuantifier = new ExistentialQuantifier(boundVariable);
            existentialQuantifier.LeftSuccessor = predicate;

            Negation negatedExistentialQuantifier = new Negation();
            negatedExistentialQuantifier.LeftSuccessor = existentialQuantifier;

            HashSet<Proposition> propositions = new HashSet<Proposition>()
            {
                negatedExistentialQuantifier
            };

            // Act
            char availableReplacementVariable = 'd';
            SemanticTableauxElement semanticTableauxElement = new SemanticTableauxElement(propositions, new HashSet<char>() { availableReplacementVariable });

            // Assert
            semanticTableauxElement.LeftChild.Should().NotBeNull("Because children should be created now that a replacement variable is present");

            foreach (Proposition proposition in semanticTableauxElement.LeftChild.Propositions)
            {
                if (proposition is Predicate)
                {
                    Predicate pred = (Predicate)proposition;
                    bool isReplaced = pred.IsReplaced(boundVariable);
                    isReplaced.Should().BeTrue($"Because the replacement variable {availableReplacementVariable} is available");
                }
            }
        }
    }
}
