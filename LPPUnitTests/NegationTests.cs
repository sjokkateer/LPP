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

            // Assert
            copy.Equals(negation).Should().BeFalse("because a copy has a different object reference");
            copy.Should().BeOfType<Negation>("because it is a copy of a negation object");
        }

        [Fact]
        // Thus ~(SYMBOL) will get shape of SYMBOL % SYMBOL
        public void Nandify_CallToNandifyWithPropositionAsSuccessor_ShouldReturnNandRootWithTerminalNodesEqualToPropositionSymbol()
        {
            // Arrange
            // Act
            Proposition nandifiedNegation = negation.Nandify();
            Nand nand = (Nand)nandifiedNegation;

            // Assert
            nandifiedNegation.Should().BeOfType<Nand>("because the expression is nandified");

            // This is a fairly fragile part in case someone were to udpate the tests to more complex test cases?
            nand.LeftSuccessor.Should().Be(negation.LeftSuccessor, "because terminal nodes will be equal to the symbol");
            nand.RightSuccessor.Should().Be(negation.LeftSuccessor, "because terminal nodes will be equal to the symbol");
        }

        [Fact]
        public void Nandify_CallToNandifyWithConnectiveAsSuccessor_ShouldReturnNandRootWithNandSuccessors()
        {
            Negation anotherNegation = new Negation();
            anotherNegation.LeftSuccessor = negation;

            Proposition nandifiedNegationWithNestedNegation = anotherNegation.Nandify();
            Nand nand = (Nand)nandifiedNegationWithNestedNegation;

            nandifiedNegationWithNestedNegation.Should().BeOfType<Nand>("because the expression is nandified");

            nand.LeftSuccessor.Should().BeOfType<Nand>("because the inner expression is nandified");
            nand.RightSuccessor.Should().BeOfType<Nand>("because the inner expression is nandified");
        }
    }
}
