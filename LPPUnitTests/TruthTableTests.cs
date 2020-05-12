using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Moq;
using LogicAndSetTheoryApplication;
using FluentAssertions.Primitives;
using Xunit;

namespace LPPUnitTests
{
    public class TruthTableTests
    {

        public void Method_SUT_Should()
        {

        }

        [Fact]
        public void GetConvertedResultColumn()
        {
            // Arrange
            int NUMBER_OF_ROWS = (new Random()).Next(1, 5);
            List<ITruthTableRow> ttrs = GenerateXTruthTableRowMocks(NUMBER_OF_ROWS);
            TruthTable tt = new TruthTable(ttrs);

            // Act
            List<int> convertedResultColumn = tt.GetConvertedResultColumn();

            int resultValue;
            ITruthTableRow currentRow;
            for (int i = 0; i < convertedResultColumn.Count; i++)
            {
                resultValue = convertedResultColumn[i];
                currentRow = ttrs[i];
                
                // Assert
                resultValue.Should().Be(Convert.ToInt32(currentRow.Result), "Because the resulting truth value should reflect the according integer value");
            }
        }

        private List<ITruthTableRow> GenerateXTruthTableRowMocks(int numberOfRows)
        {
            List<ITruthTableRow> ttrs = new List<ITruthTableRow>();

            for (int i = 0; i < numberOfRows; i++)
            {
                var ttr = new Mock<ITruthTableRow>();
                ttr.SetupGet(t => t.Result).Returns(GetRandomTruthValue());
                ttrs.Add(ttr.Object);
            }

            return ttrs;
        }

        private bool GetRandomTruthValue()
        {
            Random rng = new Random();

            return Convert.ToBoolean(rng.Next(0, 1));
        }
    }
}
