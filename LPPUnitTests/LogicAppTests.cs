using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using FluentAssertions;
using LogicAndSetTheoryApplication;

namespace LPPUnitTests
{
    public class LogicAppTests
    {
        private LogicApp logicApp;

        public LogicAppTests()
        {
            logicApp = new LogicApp(new Parser());
        }

        // Variables only
        [Theory]
        [InlineData("=(>(A, A), ~(A))", 1)]
        [InlineData(">(&(&(|(P, Q), >(P, R)), >(Q, R)), R)", 3)]
        [InlineData("&(Z, ~(Z))", 1)]
        [InlineData("&(&(>(A, A), |(B, ~(B))), ~(>(|(C, >(C, D)), |(&(A, C), D))))", 4)]
        public void Parse_ValidPropositionsWithLiterals_LogicAppShouldHaveAssignedAllRelatedProperties(string propositionExpression, int expectedNumberOfVariables)
        {
            // Arrange // Act
            logicApp.Parse(propositionExpression);
            int actualNumberOfVariables = logicApp.Variables.Count;

            // Assert
            actualNumberOfVariables.Should().Be(expectedNumberOfVariables, "Because the every unique variable should be added to the list");

            string propositionRelatedMessage = "Because the proposition should be successfully parsed an assigned";
            logicApp.Root.Should().NotBeNull(propositionRelatedMessage);
            logicApp.DisjunctiveNormalForm.Should().NotBeNull(propositionRelatedMessage);
            logicApp.Nandified.Should().NotBeNull(propositionRelatedMessage);

            string truthTableRelatedMessage = "Because the truthtables should be successfully created and assigned after parsing";
            logicApp.TruthTable.Should().NotBeNull(truthTableRelatedMessage);
            logicApp.SimplifiedTruthTable.Should().NotBeNull(truthTableRelatedMessage);

            logicApp.HashCodes.Count.Should().Be(LogicApp.hashCodeLabels.Count, "Because for each proposition a hash code should be generated");
        }

        // Constants only
        [Theory]
        [InlineData("|(&(1, 0), 1)")]
        [InlineData("&(&(>(1, 1), |(0, 1)), ~(>(|(1, >(0, 1)), |(&(1, 1), 0))))")]
        [InlineData("0")]
        [InlineData("1")]
        [InlineData("~(0)")]
        public void Parse_ValidPropositionsWithConstantsOnly_LogicAppShouldHaveRootAndTruthTablesAssigned(string constantPropositionExpression)
        {
            // Arrange // Act
            logicApp.Parse(constantPropositionExpression);
            int actualNumberOfVariables = logicApp.Variables.Count;
            int expectedNumberOfVariables = 0;

            // Assert
            actualNumberOfVariables.Should().Be(expectedNumberOfVariables, "Because the expression contains no proposition variables");
            logicApp.Root.Should().NotBeNull("Because the constant proposition should be successfully parsed an assigned");
            
            string truthTableRelatedMessage = "Because the truthtables should be successfully created and assigned after parsing";
            logicApp.TruthTable.Should().NotBeNull(truthTableRelatedMessage);
            logicApp.SimplifiedTruthTable.Should().NotBeNull(truthTableRelatedMessage);
        }

        // Mix of constants and variable
        [Theory]
        [InlineData("|(&(1, 0), A)")]
        [InlineData("&(&(>(A, A), |(0, ~(B))), ~(>(|(C, >(C, 1)), |(&(A, C), D))))")]

        public void Parse_ValidPropositionsWithLiteralsAndConstants_LogicAppShouldHaveAssignedAllRelatedProperties(string propositionExpression)
        {
            // Arrange // Act
            logicApp.Parse(propositionExpression);

            // Assert
            string propositionRelatedMessage = "Because the proposition should be successfully parsed an assigned";
            logicApp.Root.Should().NotBeNull(propositionRelatedMessage);
            logicApp.DisjunctiveNormalForm.Should().NotBeNull(propositionRelatedMessage);
            logicApp.Nandified.Should().NotBeNull(propositionRelatedMessage);

            string truthTableRelatedMessage = "Because the truthtables should be successfully created and assigned after parsing";
            logicApp.TruthTable.Should().NotBeNull(truthTableRelatedMessage);
            logicApp.SimplifiedTruthTable.Should().NotBeNull(truthTableRelatedMessage);

            logicApp.HashCodes.Count.Should().Be(LogicApp.hashCodeLabels.Count, "Because for each proposition a hash code should be generated");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Parse_InvalidExpressionGiven_ExpectedArgumentExceptionThrown(string propositionExpression)
        {
            // Arrange
            Action act = () => logicApp.Parse(propositionExpression);

            // Act // Assert
            act.Should().Throw<ArgumentException>("Because an invalid argument is given");
        }

        [Fact]
        public void Parse_NullGiven_ExpectedArgumentExceptionThrown()
        {
            // Arrange
            Proposition invalidProposition = null;
            Action act = () => logicApp.Parse(invalidProposition);

            // Act // Assert
            act.Should().Throw<ArgumentException>("Because an invalid argument is given");
        }

        [Theory]
        [InlineData("=(>(A, A), ~(A))")]
        [InlineData(">(&(&(|(P, Q), >(P, R)), >(Q, R)), R)")]
        [InlineData("&(Z, ~(Z))")]
        [InlineData("&(&(>(A, A), |(B, ~(B))), ~(>(|(C, >(C, D)), |(&(A, C), D))))")]
        public void HashCodesMatched_ValidExpressionWithVariablesOnlyGiven_ExpectedTrueReturned(string propositionExpression)
        {
            // Arrange
            logicApp.Parse(propositionExpression);

            // Act
            bool actualHashCodesMatchedUp = logicApp.HashCodesMatched();

            // Assert
            actualHashCodesMatchedUp.Should().BeTrue("Because all hash codes should match up in since every proposition would be logically equivalent");
        }

        [Theory]
        [InlineData("|(&(1, 0), 1)")]
        [InlineData("&(&(>(1, 1), |(0, 1)), ~(>(|(1, >(0, 1)), |(&(1, 1), 0))))")]
        [InlineData("0")]
        public void HashCodesMatched_ValidExpressionWithConstantsOnlyGiven_ExpectedTrueReturned(string propositionExpression)
        {
            // Arrange
            logicApp.Parse(propositionExpression);

            // Act
            bool actualHashCodesMatchedUp = logicApp.HashCodesMatched();

            // Assert
            actualHashCodesMatchedUp.Should().BeTrue("Because all hash codes should match up in since every proposition would be logically equivalent");
        }

        [Theory]
        [InlineData("|(&(1, 0), A)")]
        [InlineData("&(&(>(A, A), |(0, ~(B))), ~(>(|(C, >(C, 1)), |(&(A, C), D))))")]
        public void HashCodesMatched_ValidExpressionWithBothConstantsAndVariablesGiven_ExpectedTrueReturned(string propositionExpression)
        {
            // Arrange
            logicApp.Parse(propositionExpression);

            // Act
            bool actualHashCodesMatchedUp = logicApp.HashCodesMatched();

            // Assert
            actualHashCodesMatchedUp.Should().BeTrue("Because all hash codes should match up in since every proposition would be logically equivalent");
        }
    }
}
