using FluentAssertions;
using LogicAndSetTheoryApplication;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;


namespace LPPUnitTests
{
    public class SemanticTableauxTests
    {
        private Parser parser;

        [Fact]
        public void Constructor_PassingNullAsProposition_ShouldResultInNullReferenceExceptionThrown()
        {
            // Arrange
            Action act = () => new SemanticTableaux(null);

            // Act // Assert
            act.Should().Throw<NullReferenceException>("Because the semantic tableaux requires a proposition to evaluate");
        }

        [Fact]
        public void Constructor_ParsedPropositionGiven_ShouldResultInNegatedPropositionAsRoot()
        {
            // Arrange
            Proposition randomValidProposition = PropositionGenerator.GetRandomProposition();
            SemanticTableaux semanticTableaux = new SemanticTableaux(randomValidProposition);

            // Act
            Proposition root = semanticTableaux.Proposition;

            // Assert
            root.Should().Be(randomValidProposition, "Because the original expression is being evaluated");
        }

        [Theory]
        [InlineData("|(A, ~(A))")]
        [InlineData(">(>(|(P, Q), R), |(>(P, R), >(Q, R)))")]
        [InlineData("|(P,~(&(P, Q)))")]
        [InlineData("&(|(A, ~(A)), |(B, ~(B)))")]
        [InlineData(">(&(&(|(P, Q), >(P, R)), >(Q, R)), R)")] // Case distinction
        [InlineData(">(~(Q), ~(&(P, Q)))")]
        [InlineData(">(~(&(P, Q)), >(P, ~(Q)))")]
        [InlineData(">(&(P, ~(Q)), ~(>(P, Q)))")]
        [InlineData(">(>(P, ~(Q)), >(>(P, Q), ~(P)))")]
        [InlineData(">(>(>(P, Q), ~(P)), >(P, ~(Q)))")]
        [InlineData(">(&(P, Q), >(~(P), R))")]
        [InlineData(">(~(P), ~(>(>(P, Q), P)))")]
        [InlineData(">(>(~(P), P), P)")]
        [InlineData(">(~(>(P, Q)), P)")]
        [InlineData("|(>(P, Q), P)")]
        [InlineData("|(>(P, Q), ~(Q))")]
        [InlineData(">(=(P, Q), |(&(P, Q), &(~(P), ~(Q))))")]
        [InlineData(">(=(P, Q), =(~(P), ~(Q)))")]
        [InlineData("=(&(P, >(Q, R)), >(>(P, Q), &(P, R)))")]
        [InlineData(">(|(P, Q), |(P, &(~(P), Q)))")]
        [InlineData(">(>(P, Q), |(Q, >(P, R)))")]
        [InlineData(">(!x.(@y.(P(x, y))), @q.(!p.(P(p, q))))")]
        [InlineData(">(@x.(&(P(x), R(x))), @y.(P(y)))")]
        [InlineData(">(@x.(>(P(x), Q(x))), >(@y.(P(y)), @z.(Q(z))))")]
        public void IsClosed_TautologyGiven_ShouldResultInTrue(string tautologyExpression)
        {
            // Arrange
            parser = new Parser(tautologyExpression);

            Proposition tautology = parser.Parse();
            SemanticTableaux semanticTableaux = new SemanticTableaux(tautology);

            // Act
            bool closed = semanticTableaux.IsClosed();

            // Assert
            closed.Should().BeTrue("Because the given proposition is a tautology and therefore the tableaux should close all its branches.");
        }

        [Theory]
        [InlineData("&(P, ~(P))")]
        public void IsClosed_ContradictionGiven_ShouldResultInFalse(string contradictionExpression)
        {
            // Arrange
            parser = new Parser(contradictionExpression);

            Proposition contradiction = parser.Parse();
            SemanticTableaux semanticTableaux = new SemanticTableaux(contradiction);

            // Act
            bool closed = semanticTableaux.IsClosed();

            // Assert
            // Even though that is not always the case, since contingencies can also open in all branches.
            // Contradictions could then still be tested by evaluating all truth values and check if the result column is 0s
            closed.Should().BeFalse("Because the given proposition is a contradiction and therefore the tableaux should not be closed, and ALL branches should be open.");
        }

        [Theory]
        [InlineData("~(>(|(P, ~(P)), P)")]
        [InlineData("&(&(>(A, A), |(B, ~(B))), ~(>(|(C, >(C, D)), |(&(A, C), D))))")]
        [InlineData("|(A, B)")]
        [InlineData("=(A, B)")]
        [InlineData(">(A, B)")]
        [InlineData("&(A, B)")]
        [InlineData("~(A)")]
        [InlineData("%(A, B)")]
        [InlineData(">(>(P, Q), |(P, >(Q, R)))")]
        public void IsClosed_ContingentPropositionGiven_ShouldResultInFalse(string contingencyExpression)
        {
            // Arrange
            parser = new Parser(contingencyExpression);

            Proposition contingency = parser.Parse();
            SemanticTableaux semanticTableaux = new SemanticTableaux(contingency);

            // Act
            bool closed = semanticTableaux.IsClosed();

            // Assert
            closed.Should().BeFalse("Because the given proposition is a contingency and therefore the tableaux could have some branches closed and some open.");
        }

        [Theory]
        [InlineData(">(@q.(!p.(P(p, q))), !x.(@y.(P(x, y))))")]
        [InlineData("@x.(!y.(P(x, y)))")]
        public void IsClosed_NonClosablePredicateGiven_ShouldResultInFalse(string nonClosablePredicate)
        {
            // Arrange
            parser = new Parser(nonClosablePredicate);

            Proposition predicate = parser.Parse();
            SemanticTableaux semanticTableaux = new SemanticTableaux(predicate);

            // Act
            bool closed = semanticTableaux.IsClosed();

            // Assert
            closed.Should().BeFalse("Because the given predicate can not be closed.");
        }
    }
}
