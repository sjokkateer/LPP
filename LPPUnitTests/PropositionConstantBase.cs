using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using FluentAssertions;
using LogicAndSetTheoryApplication;

namespace LPPUnitTests
{
    abstract public class PropositionConstantBase
    {
        private readonly Proposition constant; 

        public PropositionConstantBase(Proposition constant)
        {
            this.constant = constant;
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Calculate_CallToCalculateWithDifferentTruthValues_ShouldAlwaysReturnFalse(bool truthValue)
        {
            // Arrange
            constant.TruthValue = truthValue;

            // Act
            bool actualTruthValue = constant.Calculate();

            // Assert
            ActualTruthValueShouldBe(actualTruthValue);
        }

        abstract public void ActualTruthValueShouldBe(bool actualTruthValue);

        [Fact]
        public void GetVariables_CallToGetVariables_ShouldReturnEmptyListSinceConstantHasNoVariableSymbols()
        {
            // Arrange
            int expectedNumberOfVariables = 0;

            // Act
            List<Proposition> actualVariablesInProposition = constant.GetVariables();

            // Assert
            actualVariablesInProposition.Count.Should().Be(expectedNumberOfVariables, "because a constant has no variable symbols in its expression");
        }

        [Fact]
        public void Nandify_CallToNandifyOnConstant_ShouldReturnThePropositionItself()
        {
            // Arrange
            // Act
            Proposition nandifiedConstant = constant.Nandify();

            // Assert
            nandifiedConstant.Should().BeEquivalentTo(constant, "because false nandified is still false");
        }

        [Fact]
        public void Copy_CallToCopyOnConstant_ExpectedEqualsToReturnTrueAndDifferentReferences()
        {
            // Arrange
            Proposition copy = constant.Copy();

            // Act
            bool equal = constant.Equals(copy);
            bool sameReference = constant == copy;

            // Assert
            equal.Should().BeTrue("Because it is a copy");
            sameReference.Should().BeFalse("Because it is a copy and thus should not be the same address in memory");
        }
    }
}
