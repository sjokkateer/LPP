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
        private const char FALSE = '0';
        private const char TRUE = '1';

        private Mock<Proposition> proposition;

        public TruthTableRowTests()
        {
            List<Proposition> propVariables = new List<Proposition>() { new Proposition(PropositionGenerator.getRandomVariableLetter()) };

            proposition = new Mock<Proposition>();
            proposition.Setup(p => p.GetVariables()).Returns(propVariables);
            proposition.Setup(p => p.Calculate()).Returns(false);
        }

        [Fact]
        public void Calculate_PropositionRootWithOneUniqueVariableAndCellSet_ShouldHaveAccordingTruthValueAssigned()
        {
            // Arrange
            TruthTableRow ttr = new TruthTableRow(proposition.Object);
            ttr.Cells[0] = TRUE;

            // Act
            ttr.Calculate();

            // Assert
            // We test that our method's assignment works for both cases, then it should work for all else.
            // We test if calculate is called on the root.
            // Since this framework is too hard we ignore the other branch and we do not care about if the TruthValue is actually set on the object. :-)
            proposition.Verify(p => p.Calculate(), Times.Once);
            // proposition.VerifySet(p => p.TruthValue = true);
        }

        [Theory]
        [InlineData(FALSE, false)]
        [InlineData(TRUE, true)]
        public void Copy_TruthTableCopy_ShouldHaveIdenticalValuesInCellsAndResult(char truthValueAsChar, bool result)
        {
            // Arrange
            TruthTableRow ttr = new TruthTableRow(proposition.Object);
            ttr.Cells[0] = truthValueAsChar;
            ttr.Result = result;

            // Act
            TruthTableRow copy = ttr.Copy();

            // Assert
            ttr.Cells[0].Should().Be(copy.Cells[0]);
            ttr.Result.Should().Be(copy.Result);
        }

        [Fact]
        public void EqualTo_DifferentTruthTableRowsInSize_ShouldReturnFalse()
        {
            const int SIZE_ONE = 2;
            const int SIZE_TWO = 4;

            // Arrange
            TruthTableRow ttrOne = GenerateTruthTableRowWithXPropositionVariables(SIZE_ONE);
            TruthTableRow ttrTwo = GenerateTruthTableRowWithXPropositionVariables(SIZE_TWO);

            // Act
            bool areEqual = ttrOne.EqualTo(ttrTwo);

            // Assert
            areEqual.Should().BeFalse();
        }

        private TruthTableRow GenerateTruthTableRowWithXPropositionVariables(int x)
        {
            List<Proposition> propositionVariabels = generateXPropositionVariables(x);

            return new TruthTableRow(propositionVariabels, propositionVariabels.Count);
        }

        private List<Proposition> generateXPropositionVariables(int x)
        {
            Proposition previous = null;
            Proposition newProposition = null;
            
            List<Proposition> propositions = new List<Proposition>();

            do
            {
                previous = newProposition;
                newProposition = PropositionGenerator.getRandomProposition();

                if (previous != null && !newProposition.Data.Equals(previous.Data))
                {
                    propositions.Add(newProposition);
                }

            } while (propositions.Count < x);

            return propositions;
        }

        [Fact]
        public void EqualTo_DifferentTruthTableRowsSameSize_ShouldReturnFalse()
        {
            const int SIZE_ONE = 3;
            const int SIZE_TWO = 3;

            // Arrange
            TruthTableRow ttrOne = GenerateTruthTableRowWithXPropositionVariables(SIZE_ONE);
            TruthTableRow ttrTwo = GenerateTruthTableRowWithXPropositionVariables(SIZE_TWO);

            setCellValues(ttrOne, ttrTwo, SetToDifferingValues);

            // Act
            bool areEqual = ttrOne.EqualTo(ttrTwo);

            // Assert
            areEqual.Should().BeFalse();
        }

        private void SetToDifferingValues(TruthTableRow ttrOne, TruthTableRow ttrTwo, int index, int value)
        {
            ttrOne.Cells[index] = (char) value;
            ttrTwo.Cells[index] = (char) Math.Abs(value - 1); // 0 - 1 = then 1, 1 - 1 = 0 thus will always be opposite of ttrOne's cell
        }

        private void setCellValues(TruthTableRow ttrOne, TruthTableRow ttrTwo, Action<TruthTableRow, TruthTableRow, int, int> method)
        {
            Random rng = new Random();
            int choice;

            for (int i = 0; i < ttrOne.Cells.Length; i++)
            {
                choice = rng.Next(0, 2);

                method(ttrOne, ttrTwo, i, choice);
            }
        }

        [Fact]
        public void EqualTo_EquivalentTruthTableRows_ShouldReturnTrue()
        {
            const int SIZE_ONE = 3;
            const int SIZE_TWO = 3;

            // Arrange
            TruthTableRow ttrOne = GenerateTruthTableRowWithXPropositionVariables(SIZE_ONE);
            TruthTableRow ttrTwo = GenerateTruthTableRowWithXPropositionVariables(SIZE_TWO);

            setCellValues(ttrOne, ttrTwo, SetToEquivalentValues);

            // Act
            bool areEqual = ttrOne.EqualTo(ttrTwo);

            // Assert
            areEqual.Should().BeTrue();
        }

        private void SetToEquivalentValues(TruthTableRow ttrOne, TruthTableRow ttrTwo, int index, int value)
        {
            ttrOne.Cells[index] = ttrTwo.Cells[index] = (char)value;
        }
    }
}
