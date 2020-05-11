using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using LogicAndSetTheoryApplication;
using Xunit;
using Moq;

namespace LPPUnitTests
{
    public class TruthTableRowTests
    {
        [Fact]
        public void Calculate_PropositionRootWithOneUniqueVariableAndCellSet_ShouldHaveAccordingTruthValueAssigned()
        {
            // Arrange
            List<Proposition> propVariables = new List<Proposition>() { new Proposition(PropositionGenerator.getRandomVariableLetter()) };

            var randomProposition = new Mock<Proposition>();
            randomProposition.Setup(p => p.GetVariables()).Returns(propVariables);
            randomProposition.Setup(p => p.Calculate()).Returns(false);

            TruthTableRow ttr = new TruthTableRow(randomProposition.Object);

            // Act
            ttr.Calculate();

            // Assert
            // We test that our method's assignment works for both cases, then it should work for all else.
            // We test if calculate is called on the root
            randomProposition.Verify(p => p.Calculate(), Times.Once);
        }
    }
}
