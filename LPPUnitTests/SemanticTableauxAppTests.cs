using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using LogicAndSetTheoryApplication;
using Xunit;

namespace LPPUnitTests
{
    public class SemanticTableauxAppTests
    {
        private SemanticTableauxApp semanticTableauxApp;
        
        public SemanticTableauxAppTests()
        {
            semanticTableauxApp = new SemanticTableauxApp(new Parser());
        }

        [Theory]
        [InlineData("=(>(A, A), ~(A))")]
        [InlineData(">(&(&(|(P, Q), >(P, R)), >(Q, R)), R)")]
        [InlineData("&(Z, ~(Z))")]
        [InlineData("&(&(>(A, A), |(B, ~(B))), ~(>(|(C, >(C, D)), |(&(A, C), D))))")]
        [InlineData("|(&(1, 0), 1)")]
        [InlineData("&(&(>(1, 1), |(0, 1)), ~(>(|(1, >(0, 1)), |(&(1, 1), 0))))")]
        [InlineData("0")]
        [InlineData("|(&(1, 0), A)")]
        [InlineData("&(&(>(A, A), |(0, ~(B))), ~(>(|(C, >(C, 1)), |(&(A, C), D))))")]
        // To be extended upcoming week by quantifiers
        public void Parse_ValidPropositionExpressionGiven_ExpectedTableauxAndRootToBeNotNull(string propositionExpression)
        {
            // Act
            semanticTableauxApp.Parse(propositionExpression);

            // Assert
            semanticTableauxApp.Root.Should().NotBeNull("Because the proposition should have been parsed and assigned.");
            semanticTableauxApp.SemanticTableaux.Should().NotBeNull("Because the tableaux should have been created and assigned.");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Parse_InvalidExpressionGiven_ExpectedArgumentExceptionThrown(string propositionExpression)
        {
            // Arrange
            Action act = () => semanticTableauxApp.Parse(propositionExpression);

            // Act // Assert
            act.Should().Throw<ArgumentException>("Because an invalid argument is given");
        }

        [Fact]
        public void IsTautology_TautologyParsedThrough_ExpectedTrueReturned()
        {
            // Arrange
            string tautologyExpression = ">(=(P, Q), |(&(P, Q), &(~(P), ~(Q))))";
            semanticTableauxApp.Parse(tautologyExpression);

            // Act
            bool tautology = semanticTableauxApp.IsTautology();

            // Assert
            tautology.Should().BeTrue("Because the given expression is a tautology");
        }

        [Fact]
        public void IsTautology_ContradictionParsed_ExpectedFalseReturned()
        {
            // Arrange
            string contradictionExpression = "&(P, ~(P))";
            semanticTableauxApp.Parse(contradictionExpression);

            // Act
            bool tautology = semanticTableauxApp.IsTautology();

            // Assert
            tautology.Should().BeFalse("Because the given expression is a contradiction");
        }

        [Fact]
        public void IsTautology_ContingencyParsed_ExpectedFalseReturned()
        {
            // Arrange
            string contingencyExpression = "&(&(>(A, A), |(B, ~(B))), ~(>(|(C, >(C, D)), |(&(A, C), D))))";
            semanticTableauxApp.Parse(contingencyExpression);

            // Act
            bool tautology = semanticTableauxApp.IsTautology();

            // Assert
            tautology.Should().BeFalse("Because the given expression is a contingency");
        }
    }
}
