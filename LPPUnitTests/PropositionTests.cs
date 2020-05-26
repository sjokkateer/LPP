using FluentAssertions;
using LogicAndSetTheoryApplication;
using System;
using System.Collections.Generic;
using Xunit;

namespace LPPUnitTests
{
    public class PropositionTests
    {
        private static Proposition VALID_PROPOSITION = new Proposition('A');
        private static int SAME_POSITION = 0;

        [Theory]
        [InlineData('A', "A")]
        [InlineData('L', "L")]
        [InlineData('Q', "Q")]
        [InlineData('X', "X")]
        public void ToString_ValidPropositionWithSpecificSymbol_ExpectedDataAsString(char symbol, string expectedToString)
        {
            // Arrange
            Proposition proposition = new Proposition(symbol);

            // Act
            string actualToString = proposition.ToString();

            // Assert
            actualToString.Should().Be(expectedToString, "Because for a proposition only the symbol itself is returned.");
        }

        [Fact]
        public void Constructor_ConstructPropositionWithValidCharacterRepresentingVariable_ExpectedObjectToHoldCharacterAsVariable()
        {
            // Arrange
            char randomValidVariableLetter = PropositionGenerator.GetRandomVariableLetter();

            // Act
            Proposition proposition = new Proposition(randomValidVariableLetter);

            // Assert
            proposition.Data.Should().BeEquivalentTo(randomValidVariableLetter, "because the random variable letter should be assigned to the data field by the constructor");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Calculate_AssignTruthValueToProposition_ExpectedAssignedTruthValueReturned(bool truthValue)
        {
            // Arrange
            VALID_PROPOSITION.TruthValue = truthValue;

            // Act
            bool actualResult = VALID_PROPOSITION.Calculate();

            // Assert
            actualResult.Should().Be(truthValue, "because the assigned truthValue should be returned");
        }

        [Fact]
        public void Calculate_NoTruthValueAssigned_ExpectedFalseReturned()
        {
            // Arrange 
            Proposition proposition = new Proposition('A');
            
            // Act
            bool actualResult = proposition.Calculate();

            // Assert
            actualResult.Should().BeFalse("because TruthValue was never assigned and defaults to false");
        }

        [Fact]
        public void Copy_CopyAValidPropositionObject_ExpectedDifferentReferenceSameDataVariable()
        {
            // Arrange // Act
            Proposition copy = VALID_PROPOSITION.Copy();

            // Assert
            copy.Should().NotBeSameAs(VALID_PROPOSITION, "because it is a copy");
            copy.Data.Should().BeEquivalentTo(VALID_PROPOSITION.Data, "because it is a copy");
        }

        [Fact]
        public void GetVariables_CallGetVariablesOnARandomValidProposition_ExpectedListReturnedWithTheVariableAsIndividualElement()
        {
            // Arrange 
            Proposition validProposition = PropositionGenerator.GetRandomPropositionSymbol();

            int expectedNumberOfElements = 1;

            // Act
            List<Proposition> propositionVariables = validProposition.GetVariables();

            // Assert
            propositionVariables.Count.Should().Be(expectedNumberOfElements, "because the proposition variable is an individual variable");
            propositionVariables[0].Should().BeEquivalentTo(validProposition, "because the proposition variable itself is the actual proposition");
        }

        [Fact]
        public void ToString_CallToStringOnValidVariable_ExpectedTheVariableDataReturned()
        {
            // Arrange
            string expectedResultString = "A";

            // Act
            string actualResultString = VALID_PROPOSITION.ToString();

            // Assert
            actualResultString.Should().BeEquivalentTo(expectedResultString, "because the proposition returns its data as string");
        }

        [Fact]
        public void Nandify_CallToNandifyOnValidRandomVariable_ExpectedNandifiedPropositionReturned()
        {
            // Arrange
            Proposition validProposition = PropositionGenerator.GetRandomPropositionSymbol();

            // Act
            Proposition nandifiedProposition = validProposition.Nandify();

            // Assert
            // The leaves are 'variable symbols' and the non terminal nodes are nand connectives.
            List<Proposition> queue = new List<Proposition>() { nandifiedProposition };
            Proposition current;

            // BFS Traversal over the proposition tree
            do
            {
                current = pop(queue);

                if (current is BinaryConnective) 
                {
                    // Assert non terminal node is a Nand connective.
                    current.Should().BeOfType<Nand>("because the proposition is being nandified");
                    addChildren((BinaryConnective)current, queue);
                }
                else
                {
                    // Assert that terminal nodes are the proposition variable.
                    current.Should().BeEquivalentTo(validProposition, "because the proposition variable is a terminal node");
                }

            } while (queue.Count > 0);
        }

        private Proposition pop(List<Proposition> queue)
        {
            int head = 0;
            Proposition firstSymbol = null;

            if (queue.Count > 0)
            {
                firstSymbol = queue[head];
                queue.RemoveAt(head);
            }

            return firstSymbol;
        }

        private void addChildren(BinaryConnective binaryConnective, List<Proposition> queue)
        {
            Proposition leftSuccessor = binaryConnective.LeftSuccessor;
            Proposition rightSuccessor = binaryConnective.RightSuccessor;

            if (leftSuccessor != null)
            {
                queue.Add(leftSuccessor);
            }

            if (rightSuccessor != null)
            {
                queue.Add(rightSuccessor);
            }
        }

        [Fact]
        public void CompareTo_TwoPropositionsOfWhichOtherIsNull_ExpectedArgumentNullExceptionThrown()
        {
            // Arrange
            Proposition invalidOtherProposition = null;

            // Act
            Action act = () => VALID_PROPOSITION.CompareTo(invalidOtherProposition);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void CompareTo_TwoPropositionsOfIdenticalVariable_ExpectedToBeEquivalent()
        {
            // Arrange
            Proposition proposition = new Proposition('T');
            Proposition otherProposition = new Proposition('T');

            // Act
            int samePosition = proposition.CompareTo(otherProposition);

            // Assert
            samePosition.Should().Be(SAME_POSITION, "because their variables are the same and thus come in the same position");
        }

        [Fact]
        public void CompareTo_ThreePropositionsFirstBeforeSecondSecondBeforeThrid_ExpectedFirstToComeBeforeSecondAndThird()
        {
            // Arrange
            Proposition firstInOrderproposition = new Proposition('S');
            Proposition secondInOrderProposition = new Proposition('T');
            Proposition thirdInOrderProposition = new Proposition('X');

            // Act
            // Transitive relation
            int firstBeforeSecond = firstInOrderproposition.CompareTo(secondInOrderProposition);
            int secondBeforeThird = secondInOrderProposition.CompareTo(thirdInOrderProposition);
            int firstBeforeThrid = firstInOrderproposition.CompareTo(thirdInOrderProposition);

            // Assert
            firstBeforeSecond.Should().BeLessThan(SAME_POSITION, "because S comes before T");
            secondBeforeThird.Should().BeLessThan(SAME_POSITION, "because T comes before X");
            firstBeforeThrid.Should().BeLessThan(SAME_POSITION, "because S < T and T < X");
        }
    }
}
