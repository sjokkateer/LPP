using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using LogicAndSetTheoryApplication;
using FluentAssertions;
using FluentAssertions.Formatting;
using System.Linq;

namespace LPPUnitTests
{
    public class HashCodeCalculatorTests
    {
        private HashCodeCalculator hashCodeCalculator;

        public HashCodeCalculatorTests()
        {
            hashCodeCalculator = new HashCodeCalculator(HashCodeCalculator.HEXADECIMAL);
        }

        [Fact]
        public void Constructor_ListOfBitsProvidedAndHashBase_ExpectedObjectConstructedAndHashCodeAssigned()
        {
            // Arrange
            List<int> bits = new List<int>() { 1, 1, 1, 1 };

            // Act
            HashCodeCalculator calculator = new HashCodeCalculator(bits, HashCodeCalculator.HEXADECIMAL);

            // Assert
            calculator.HashCode.Should().NotBe(string.Empty, "Because the hash code has been calculated and assigned after constructing");
        }

        [Fact]
        public void Constructor_EmptyListOfBitsProvidedAndHashBase_ExpectedObjectConstructedAndEmptyStringHashCode()
        {
            // Arrange
            List<int> bits = new List<int>();

            // Act
            HashCodeCalculator calculator = new HashCodeCalculator(bits, HashCodeCalculator.HEXADECIMAL);

            // Assert
            calculator.HashCode.Should().Be(string.Empty, "Because the hash code has been calculated and assigned after constructing");
        }

        [Fact]
        public void Constructor_NullGivenToGenerateHashCode_ExpectedArgumentExceptionThrown()
        {
            // Arrange
            List<int> bits = null;
            Action act = () => new HashCodeCalculator(bits, HashCodeCalculator.HEXADECIMAL);

            // Act // Assert
            act.Should().Throw<ArgumentException>("Because without a list of bits the hashcode can not be calculated");
        }

        [Theory]
        [InlineData("0", 0)]
        [InlineData("1", 1)]
        [InlineData("2", 0, 1)]
        [InlineData("3", 1, 1)]
        [InlineData("4", 0, 0, 1)]
        [InlineData("5", 1, 0, 1)]
        [InlineData("6", 0, 1, 1)]
        [InlineData("7", 1, 1, 1)]
        [InlineData("8", 0, 0, 0, 1)]
        [InlineData("9", 1, 0, 0, 1)]
        [InlineData("A", 0, 1, 0, 1)]
        [InlineData("B", 1, 1, 0, 1)]
        [InlineData("C", 0, 0, 1, 1)]
        [InlineData("D", 1, 0, 1, 1)]
        [InlineData("E", 0, 1, 1, 1)]
        [InlineData("F", 1, 1, 1, 1)]

        public void GenerateHashCode_ListOfOneIntegerGivenWithinTheRangeOf1To16_ExpectedCorrespondingHexaDecimalCharacterReturned(string expectedCharacter, params int[] bits)
        {
            // Arrange
            hashCodeCalculator.GenerateHashCode(bits.ToList());

            // Act
            string actualHashCode = hashCodeCalculator.HashCode;

            // Assert
            actualHashCode.Should().Be(expectedCharacter, $"Because the bits evaluated to base 16 should match the expected character {expectedCharacter}");
        }

        [Theory]
        [InlineData(2)]
        [InlineData(4)]
        [InlineData(8)]
        [InlineData(16)]
        public void SetHashBase_ValidHashBaseGiven_ExpectedHashBaseToBeAssigned(int validBase)
        {
            // Arrange // Act
            hashCodeCalculator.HashBase = validBase;

            // Assert
            hashCodeCalculator.HashBase.Should().Be(validBase, "Because a power of 2 base should be assigned");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(6)] // Should fail according to the exceptions I was interested in powers of 2, which 6 is not.
        [InlineData(125)]
        [InlineData(-10)]
        public void SetHashBase_InvalidHashBaseGiven_ExpectedArgumentExceptionThrown(int invalidBase)
        {
            // Arrange
            Action act = () => hashCodeCalculator.HashBase = invalidBase;

            // Act // Assert
            act.Should().Throw<ArgumentException>("Because a non power of 2 base is given");
        }


    }
}
