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
        private Parser parser;

        [Theory]
        [InlineData("1", 1)] // 0 ^ 2
        [InlineData("A", 2)] // 1 ^ 2
        [InlineData("|(A, B)", 4)] // n ^ 2 rows
        [InlineData("|(A, &(B, C))", 8)]
        public void Constructor_ConstructWithVariablesAlreadySetOnProposition_ShouldResultInAProperlyConstructedObjectWithANumberOfRowsEqualToNumberOfVariablesToThePowerOfTwo(String toParseExpression, int expectedNumberOfRows)
        {
            // Arrange
            parser = new Parser(toParseExpression);
            Proposition root = parser.Parse();

            // Act
            root.UniqueVariableSet = root.GetVariables();
            TruthTable tt = new TruthTable(root);
            int actualNumberOfRows = tt.Rows.Count;

            // Assert
            actualNumberOfRows.Should().Be(expectedNumberOfRows);
        }

        [Theory]
        [InlineData("A")]
        [InlineData("1")]
        [InlineData("=(A,B)")]
        public void Simplify_NonSimplifiablTruthTable_ShouldResultInTheOriginalTruthTable(String toParseExpression)
        {
            // Same number of rows
            // These should result in the same sets of rows
            parser = new Parser(toParseExpression);
            Proposition root = parser.Parse();
            TruthTable tt = new TruthTable(root);

            // Act
            TruthTable simplifiedTt = tt.Simplify();

            // Assert
            tt.Rows.Count.Should().Be(simplifiedTt.Rows.Count, "Because the expression could not be simplified");
        }

        [Theory]
        [InlineData("|(B, T)")]
        [InlineData("&(B, T)")]
        public void Simplify_SimplifiableTruthTable_ShouldResultInSimplifiedTruthTable(String toParseExpression)
        {
            // A simplified truth table has less rows than the original.
            // They are also flagged as simplified and for that reason treated differently
            // Arrange
            parser = new Parser(toParseExpression);
            Proposition root = parser.Parse();
            TruthTable tt = new TruthTable(root);

            // Act
            TruthTable simplifiedTt = tt.Simplify();

            // Assert
            tt.Rows.Count.Should().BeGreaterThan(simplifiedTt.Rows.Count, "Because the simplified truth table should have less rows as some could be simplified");
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

        // Helper
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

        // Helper
        private bool GetRandomTruthValue()
        {
            Random rng = new Random();

            return Convert.ToBoolean(rng.Next(0, 1));
        }

        [Theory]
        [InlineData("1")]
        [InlineData("A")]
        [InlineData("|(A, B)")]
        [InlineData("|(A, &(B, C))")]
        public void ToString_MultipleVariablesInTruthTable_NumberOfPiecesShouldBeEquivalentToNumberOfVariablesPlusOneForResultColumn(String toParseExpression)
        {
            // Arrange
            parser = new Parser(toParseExpression);
            Proposition root = parser.Parse();
            TruthTable tt = new TruthTable(root);

            int numberOfVariables = root.GetVariables().Count;
            int numberOfExpectedParts = numberOfVariables + 1;

            // Act
            String result = tt.TableHeader();
            String[] parts = result.Split(TruthTableRow.GetPadding());

            // Assert
            parts.Length.Should().Be(numberOfExpectedParts, "Because the string should display a value for each variable and a result");
        }

        // ToString
        // Should result in number of rows + 1 for the table header when we split on \n
        [Theory]
        [InlineData("1", 2)] // 0 ^ 2 + 1
        [InlineData("A", 3)] // # variables ^ 2 + 1 for header
        [InlineData("|(A, B)", 5)]
        [InlineData("|(A, &(B, C))", 9)]
        public void ToString_ConstantPropositionRootGivenToConstructor_ExpectedTwoRowsPrinted(String toParseExpression, int expectedNumberOfRows)
        {
            // Arrange
            parser = new Parser(toParseExpression);
            Proposition root = parser.Parse();
            TruthTable tt = new TruthTable(root);

            // Act
            String tableToString = tt.ToString();
            String[] rowsAsString = tableToString.Split("\n");

            // Assert
            rowsAsString.Length.Should().Be(expectedNumberOfRows);
        }
    }
}
