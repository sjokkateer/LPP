using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using FluentAssertions;
using LogicAndSetTheoryApplication;

namespace LPPUnitTests
{
    public class FalseTests
    {
        private readonly False f = new False();

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Calculate_CallToCalculateWithDifferentTruthValues_ShouldAlwaysReturnFalse(bool truthValue)
        {
            // Arrange
            f.TruthValue = truthValue;

            // Act
            bool actualTruthValue = f.Calculate();

            // Assert
            actualTruthValue.Should().BeFalse("becasue false is a constant false");
        }

        [Fact]
        public void GetVariables_CallToGetVariables_ShouldReturnEmptyListSinceConstantHasNoVariableSymbols()
        {
            // Arrange
            int expectedNumberOfVariables = 0;

            // Act
            List<Proposition> actualVariablesInProposition = f.GetVariables();

            // Assert
            actualVariablesInProposition.Count.Should().Be(expectedNumberOfVariables, "because a constant has no variable symbols in its expression");
        }

        [Fact]
        public void Nandify_CallToNandify_ShouldReturnThePropositionItself()
        {
            // Arrange

            // Act
            Proposition nandifiedFalse = f.Nandify();

            // Assert
            nandifiedFalse.Should().BeEquivalentTo(f, "because false nandified is still false");
        }
    }
}
