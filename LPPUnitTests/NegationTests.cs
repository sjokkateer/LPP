using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using FluentAssertions;
using LogicAndSetTheoryApplication;

namespace LPPUnitTests
{
    public class NegationTests
    {
        private Negation negation;

        public NegationTests()
        {
            negation = new Negation();
            negation.LeftSuccessor = PropositionGenerator.GetRandomPropositionSymbol();
        }

        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public void Calculate_CallToCalculateWithDifferentTruthValues_ShouldReturnTheOppositeTruthValue(bool truthValue, bool expectedTruthValue)
        {
            // Arrange
            negation.LeftSuccessor.TruthValue = truthValue;

            // Act
            bool actualTruthValue = negation.Calculate();

            // Assert
            actualTruthValue.Should().Be(expectedTruthValue, "because the negation of a truth value results in the opposite value");
        }

        [Fact]
        public void Copy_CallToCopy_ShouldReturnNewReferenceOfSameObjectType()
        {
            // Arrange
            // Act
            Proposition copy = negation.Copy();
            bool sameReference = negation == copy;

            // Assert
            sameReference.Should().BeFalse("Because the copy should be a different object reference");
            copy.Equals(negation).Should().BeTrue("Because the copy should have the same data");
        }

        [Fact]
        // Thus ~(SYMBOL) will get shape of SYMBOL % SYMBOL
        public void Nandify_CallToNandifyWithPropositionAsSuccessor_ShouldReturnNandRootWithTerminalNodesEqualToPropositionSymbol()
        {
            // Arrange
            // Act
            Proposition nandifiedNegation = negation.Nandify();

            // Assert
            NandChecker.hasNandStructure(new List<Proposition>() { nandifiedNegation });
        }

        [Fact]
        public void Nandify_CallToNandifyWithConnectiveAsSuccessor_ShouldReturnNandRootWithNandSuccessors()
        {
            // Arrange
            Negation anotherNegation = new Negation();
            anotherNegation.LeftSuccessor = negation;

            // Act
            Proposition nandifiedNegationWithNestedNegation = anotherNegation.Nandify();

            // Assert
            NandChecker.hasNandStructure(new List<Proposition>() { nandifiedNegationWithNestedNegation });
        }
    }
}
