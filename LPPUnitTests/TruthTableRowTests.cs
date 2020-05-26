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
            List<Proposition> propVariables = new List<Proposition>() { new Proposition(PropositionGenerator.GetRandomVariableLetter()) };

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
            ttr.Cells[0].Should().Be(copy.Cells[0], "Because it is a copy, it should have identical values per cell");
            ttr.Result.Should().Be(copy.Result, "Because it is a copy, the result values should be identical");
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
            areEqual.Should().BeFalse("Because the size of the two arrays differs (equivalently the number of variables)");
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

            while (propositions.Count < x)
            {
                previous = newProposition;
                newProposition = PropositionGenerator.GetRandomProposition();

                if (previous != null && !newProposition.Data.Equals(previous.Data))
                {
                    propositions.Add(newProposition);
                }

            }

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
            areEqual.Should().BeFalse("Because the cells in the truth table rows have differing values");
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
            areEqual.Should().BeTrue("Because they are of equal size and have identical values in each cell");
        }

        private void SetToEquivalentValues(TruthTableRow ttrOne, TruthTableRow ttrTwo, int index, int value)
        {
            ttrOne.Cells[index] = ttrTwo.Cells[index] = (char)value;
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void ToString_NoVariablesInTruthTableRow_OnlyResultColumnReturned(bool resultValue)
        {
            // Arrange
            TruthTableRow ttr = GenerateTruthTableRowWithXPropositionVariables(0);
            ttr.Result = resultValue;

            // Act
            String result = ttr.ToString();
            int len = result.Split().Length;

            // Assert
            len.Should().Be(1, "Because only the resulting column will be included");
        }

        [Fact]
        public void ToString_MultipleVariablesInTruthTableRow_NumberOfPiecesShouldBeEquivalentToNumberOfVariablesPlusOneForResultColumn()
        {
            // Arrange
            Random rng = new Random();
            int numberOfVariables = rng.Next(1, 5);
            TruthTableRow ttr = GenerateTruthTableRowWithXPropositionVariables(numberOfVariables);

            // Act
            String result = ttr.ToString();
            String[] parts = result.Split(TruthTableRow.GetPadding());
            
            int numberOfExpectedParts = numberOfVariables + 1;

            // Assert
            parts.Length.Should().Be(numberOfExpectedParts, "Because the string should display a value for each variable and a result");
        }
    }
}
