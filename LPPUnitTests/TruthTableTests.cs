using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Moq;
using LogicAndSetTheoryApplication;
using FluentAssertions.Primitives;
using Xunit;
using System.Collections;

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
        public void Constructor_ConstructWithVariablesAlreadySetOnProposition_ShouldResultInAProperlyConstructedObjectWithANumberOfRowsEqualToNumberOfVariablesToThePowerOfTwo(string toParseExpression, int expectedNumberOfRows)
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
        public void Simplify_NonSimplifiablTruthTable_ShouldResultInTheOriginalTruthTable(string toParseExpression)
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
        public void Simplify_SimplifiableTruthTable_ShouldResultInSimplifiedTruthTable(string toParseExpression)
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
        public void ToString_MultipleVariablesInTruthTable_NumberOfPiecesShouldBeEquivalentToNumberOfVariablesPlusOneForResultColumn(string toParseExpression)
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
        public void ToString_ConstantPropositionRootGivenToConstructor_ExpectedTwoRowsPrinted(string toParseExpression, int expectedNumberOfRows)
        {
            // Arrange
            parser = new Parser(toParseExpression);
            Proposition root = parser.Parse();
            TruthTable tt = new TruthTable(root);

            // Act
            string tableToString = tt.ToString();
            string[] rowsAsString = tableToString.Split("\n");

            // Assert
            rowsAsString.Length.Should().Be(expectedNumberOfRows);
        }

        // An individual proposition is already in DNF thus this could be an individual test
        [Fact]
        public void CreateDisjunctiveNormalForm_IndividualPropositionGiven_ExpectedIndividualPropositionReturned()
        {
            // Arrange
            Proposition originalProposition = PropositionGenerator.GetRandomProposition();
            TruthTable tt = new TruthTable(originalProposition);
            TruthTable simplifiedTt = tt.Simplify();
            
            // Act
            Proposition dnf = tt.CreateDisjunctiveNormalForm();
            Proposition simplifiedDnf = simplifiedTt.CreateDisjunctiveNormalForm();

            // Assert
            dnf.Should().BeEquivalentTo(originalProposition, "Because the disjunctive normal of a proposition literal is the literal itself");
            simplifiedDnf.Should().BeEquivalentTo(originalProposition, "Because a literal can not be simplified any further and should result in the literal itself.");
        }

        [Fact]
        public void CreateDisjunctiveNormalForm_NegatedProposition_ExpectedOriginalNegatedPropositionReturned()
        {
            // Arrange
            Proposition originalProposition = PropositionGenerator.CreateUnaryConnectiveWithRandomSymbol(Negation.SYMBOL);
            TruthTable tt = new TruthTable(originalProposition);
            TruthTable simplifiedTt = tt.Simplify();

            // Act
            Proposition dnf = tt.CreateDisjunctiveNormalForm();
            Proposition simplifiedDnf = simplifiedTt.CreateDisjunctiveNormalForm();

            // Assert
            dnf.Should().BeEquivalentTo(originalProposition, "Because the disjunctive normal of a negated proposition literal is the negated literal itself");
            simplifiedDnf.Should().BeEquivalentTo(originalProposition, "Because a negated literal can not be simplified any further and should result in the negated literal itself.");
        }

        [Theory]
        [InlineData(Implication.SYMBOL)]
        [InlineData(BiImplication.SYMBOL)]
        [InlineData(Conjunction.SYMBOL)]
        [InlineData(Disjunction.SYMBOL)]
        public void CreateDisjunctiveNormalForm_ExpressionGiven_ExpectedPropositionLiteralsAtTheLeavesAndNoDisjunctionDeeperThanConjunction(char expressionSymbol)
        {
            // For the binary connectives, we should check all (both) possible branches if they contain
            // no disjunction below a conjunction

            // Arrange
            BinaryConnective root = PropositionGenerator.CreateBinaryConnectiveWithRandomSymbols(expressionSymbol);
            TruthTable tt = new TruthTable(root);
            TruthTable simplifiedTt = tt.Simplify();

            // Act
            Proposition dnf = tt.CreateDisjunctiveNormalForm();
            Proposition simplifiedDnf = simplifiedTt.CreateDisjunctiveNormalForm();

            // Assert
            Assert.True(IsInDnf(dnf), "Because this expression should be in DNF");
            Assert.True(IsInDnf(simplifiedDnf), "Because the simplified expression should also be in DNF");
        }

        [Fact]
        // This method actually tests the logic of the helper functions for testing if a proposition is in dnf.
        // Other symbols most likely will never occure based on application logic for disjunctive normal form
        // but it is good to make sure that the helper method actually expresses the logic we want it to.
        public void CreateDisjunctiveNormalForm_ExpressionNotInDisjunctiveNormalForm_ExpectedFalseReturned()
        {
            // Arrange
            parser = new Parser("&(|(P,Q),R)");
            Proposition proposition = parser.Parse();

            // Act
            bool dnf = IsInDnf(proposition);

            // Assert
            dnf.Should().BeFalse("Because the expression does not match the pattern of an expression in disjunctive normal form.");
        }

        private bool IsInDnf(Proposition dnfRoot)
        {
            bool dnf = true;

            Stack<Proposition> propositionStack = new Stack<Proposition>();
            propositionStack.Push(dnfRoot);

            Proposition currentProposition = dnfRoot;

            while (propositionStack.Count > 0)
            {
                if (currentProposition.GetType() == typeof(Conjunction))
                {
                    Conjunction conjunction = (Conjunction)currentProposition;
                    dnf = checkIfNotFollowedByDisjunction(conjunction);

                    if (!dnf) // If it fails anywhere in the syntax tree we can early exit.
                    {
                        return dnf;
                    }
                }
                else
                {
                    AddChildrenToStack(ref propositionStack, currentProposition);
                }

                currentProposition = propositionStack.Pop();
            }

            return dnf;
        }

        private bool checkIfNotFollowedByDisjunction(Conjunction conjunction)
        {
            Stack<Proposition> propositionStack = new Stack<Proposition>();
            propositionStack.Push(conjunction);
            Proposition currentProposition = conjunction;

            while(propositionStack.Count > 0)
            {
                if (currentProposition.GetType() == typeof(Disjunction))
                {
                    return false;
                }

                AddChildrenToStack(ref propositionStack, currentProposition);
                currentProposition = propositionStack.Pop();
            }

            return true;
        }

        private void AddChildrenToStack(ref Stack<Proposition> propositionStack, Proposition proposition)
        {
            if (proposition is UnaryConnective)
            {
                UnaryConnective unaryConnective = (UnaryConnective)proposition;
                propositionStack.Push(unaryConnective.LeftSuccessor);
            }

            if (proposition is BinaryConnective)
            {
                BinaryConnective binaryConnective = (BinaryConnective)proposition;
                propositionStack.Push(binaryConnective.RightSuccessor);
            }
        }
    
        [Theory]
        [InlineData("|(&(A, &(B, ~(B))), C)", 2)]
        [InlineData("&(F, ~(F))", 1)]
        public void GetSimplifiedExpression_ExpressionWithDoNotCareVariableGiven_ExpectedDoNoCareVariableToBeRemovedFromProposition(string simplifiableExpression, int numberOfDoNotCareVariables)
        {
            // Arrange
            parser = new Parser(simplifiableExpression);
            Proposition simplifiableProposition = parser.Parse();

            TruthTable truthTable = new TruthTable(simplifiableProposition);
            TruthTable simplifiedTruthTable = truthTable.Simplify();
            Proposition afterSimplifying = simplifiedTruthTable.GetSimplifiedExpression();

            // Act
            int numberOfVariablesInOriginal = simplifiableProposition.GetVariables().Count;
            int actualNumberOfVariablesInSimplified = afterSimplifying.GetVariables().Count;

            int actualNumberOfDontCareVariables = numberOfVariablesInOriginal - actualNumberOfVariablesInSimplified;

            // Assert
            actualNumberOfDontCareVariables.Should().Be(numberOfDoNotCareVariables, $"Because the proposition could be simplified and {numberOfDoNotCareVariables} variables should be removed from the original expression that has {numberOfVariablesInOriginal} variables");
        }
        
        [Theory]
        [InlineData("&(A, &(B, C))")]
        [InlineData("U")]
        public void GetSimplifiedExpression_ExpressionWithoutDoNotCareVariableGiven_ExpectedEqualNumberOfVariablesInProposition(string nonSimplifiableExpression)
        {
            // Arrange
            parser = new Parser(nonSimplifiableExpression);
            Proposition nonSimplifiableProposition = parser.Parse();

            TruthTable truthTable = new TruthTable(nonSimplifiableProposition);
            TruthTable simplifiedTruthTable = truthTable.Simplify();
            Proposition afterSimplifying = simplifiedTruthTable.GetSimplifiedExpression();

            // Act
            int numberOfVariablesInOriginal = nonSimplifiableProposition.GetVariables().Count;
            int actualNumberOfVariablesInSimplified = afterSimplifying.GetVariables().Count;

            int actualNumberOfDontCareVariables = numberOfVariablesInOriginal - actualNumberOfVariablesInSimplified;

            // Assert
            actualNumberOfDontCareVariables.Should().Be(0, $"Because the proposition could NOT be simplified thus should have a difference of 0");
        }
    }
}
