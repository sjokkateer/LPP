using System;
using System.Collections.Generic;
using System.Text;
using LogicAndSetTheoryApplication;

namespace LPPUnitTests
{
    public class PropositionGenerator
    {
        private static int CAPITAL_A = 65;
        private static int CAPITAL_Z = 90;

        private static Random rng;

        public static BinaryConnective createBinaryConnectiveWithRandomSymbols(char binaryConnectiveSymbol)
        {
            BinaryConnective binaryConnective = null;

            switch(binaryConnectiveSymbol)
            {
                case '&':
                    binaryConnective = new Conjunction();
                    break;
                case '=':
                    binaryConnective = new BiImplication();
                    break;
                case '|':
                    binaryConnective = new Disjunction();
                    break;
                default:
                    throw new ArgumentNullException("Could not convert symbol into a connective!");
            }

            binaryConnective.LeftSuccessor = new Proposition(getRandomVariableLetter());
            binaryConnective.RightSuccessor = new Proposition(getRandomVariableLetter());

            return binaryConnective;
        }

        public static char getRandomVariableLetter()
        {
            rng = new Random();
            int randomCapitalLetter = rng.Next(CAPITAL_A, CAPITAL_Z + 1);

            return (char)randomCapitalLetter;
        }

        public static char getRandomConnective()
        {
            rng = new Random();
            int randomIndexOfConnectivesString = rng.Next(0, Parser.CONNECTIVES.Length);

            return Parser.CONNECTIVES[randomIndexOfConnectivesString];
        }
    }
}
