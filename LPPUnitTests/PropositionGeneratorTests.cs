using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using FluentAssertions;
using LogicAndSetTheoryApplication;

namespace LPPUnitTests
{
    public class PropositionGeneratorTests
    {
        [Fact]
        // This method can apparently fail on the number of 0s in the hashcode string.
        public void GenerateExpression_GenerateRandomPropositionExpression_ExpectedHashCodesToMatchUp()
        {
            int hashBase = 16;
            string message = "Because all logically equivalent propositions have exactly the same hash code.";

            // Arrange
            // Original (1)
            Proposition generatedProposition = PropositionGenerator.GenerateExpression();
            TruthTable tt = new TruthTable(generatedProposition);
            HashCodeCalculator hashCodeCalculator = new HashCodeCalculator(tt.GetConvertedResultColumn(), hashBase);

            // Original Normalized (4)
            Proposition disjunctiveProposition = tt.CreateDisjunctiveNormalForm();
            TruthTable disjunctiveTt = new TruthTable(disjunctiveProposition);
            HashCodeCalculator hashCodeCalculatorDnf = new HashCodeCalculator(disjunctiveTt.GetConvertedResultColumn(), hashBase);

            // Original Simplified Normalized (5)
            TruthTable simplifiedTt = tt.Simplify();
            Proposition simplifiedDnf = simplifiedTt.CreateDisjunctiveNormalForm();
            TruthTable simplifiedDnfTt = new TruthTable(simplifiedDnf);

            HashCodeCalculator hashCodeCalculatorSimplifiedDnf = new HashCodeCalculator(simplifiedDnfTt.GetConvertedResultColumn(), hashBase);

            // Original Simplified Normalized Nandified (6)
            Proposition simplifiedDnfNandified = simplifiedDnf.Nandify();
            TruthTable simplifiedDnfNandifiedTt = new TruthTable(simplifiedDnfNandified);
            HashCodeCalculator hashCodeCalculatorSimplifiedDnfNandified = new HashCodeCalculator(simplifiedDnfNandifiedTt.GetConvertedResultColumn(), hashBase);

            // Act
            string originalHashCode = hashCodeCalculator.HashCode;
            string dnfHashCode = hashCodeCalculatorDnf.HashCode;
            string simplifiedDnfHashCode = hashCodeCalculatorSimplifiedDnf.HashCode;
            string simplifiedDnfNandifiedHashCode = hashCodeCalculatorSimplifiedDnfNandified.HashCode;
            //string originalHashCode = hashCodeCalculator.HashCode;
            //string originalHashCode = hashCodeCalculator.HashCode;

            // Assert
            originalHashCode.Should().Be(dnfHashCode, message);
            originalHashCode.Should().Be(simplifiedDnfHashCode, message);
            originalHashCode.Should().Be(simplifiedDnfNandifiedHashCode, message);
        }
    }
}
