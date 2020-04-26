using LogicAndSetTheoryApplication;
using Xunit;

namespace LPPUnitTests
{
    public class DisjunctionTests : BinaryConnectiveTestsBase
    {
        public DisjunctionTests() : base(Disjunction.SYMBOL)
        { }

        [Theory]
        [InlineData(false, false, false)]
        [InlineData(false, true, true)]
        [InlineData(true, false, true)]
        [InlineData(true, true, true)]
        public void Calculate_CalculateAllPossibleTruthValues_ExpectedTrueWhenLeftOrRightAreTrue(bool leftTruthValue, bool rightTruthValue, bool expectedTruthValue)
        {
            // Arrange
            Disjunction disjunction = generateDisjunction();
            string message = "because disjunction is true when either left or right are true";

            Calculate_DetermineAllPossibleValuesBetweenTwoPropositionVariables(disjunction, message, leftTruthValue, rightTruthValue, expectedTruthValue);
        }

        private Disjunction generateDisjunction()
        {
            return (Disjunction)createBinaryConnective();
        }

        [Fact]
        public void Copy_CopyingDisjunctionWithTwoRandomVariableSymbols_ExpectedDifferentReferencesForConnective()
        {
            // Arrange // Act // Assert
            Disjunction disjunction = generateDisjunction();

            Copy_CopyingBinaryConnectiveWithTwoRandomVariableSymbols_ExpectedDifferentReferencesForConnective(disjunction);
        }
    }
}
