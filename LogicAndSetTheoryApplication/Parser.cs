using System.Collections.Generic;
using System.Linq;
using System;

namespace LogicAndSetTheoryApplication
{
    class Parser
    {
        private const string CONNECTIVES = "~>=&|";
        private const string VARIABLES = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        private string proposition;

        // New addition to keep track of the variables that have already been inserted.
        private List<Proposition> alreadyProcessedVariables;

        private List<char> connectives;
        private List<Proposition> symbols;

        public Parser(string proposition)
        {
            this.proposition = proposition;

            connectives = new List<char>();
            symbols = new List<Proposition>();
            alreadyProcessedVariables = new List<Proposition>();
        }

        public Proposition Parse()
        {
            ParseHelper(proposition);
            return symbols[0];
        }

        private void ParseHelper(string s)
        {
            if (s != string.Empty)
            {
                if (CONNECTIVES.Contains(s[0]))
                {
                    connectives.Add(s[0]);
                }
                else if (VARIABLES.Contains(s[0]))
                {
                    Proposition symbol = IsVariableInExpression(s[0]);
                    if (symbol == null)
                    {
                        // New abstract variable encountered, create a new symbol.
                        symbol = new Proposition(s[0]);
                        // Add the symbol by reference in the list of already processed 
                        // abstract proposition variables.
                        alreadyProcessedVariables.Add(symbol);
                    }
                    symbols.Add(symbol);
                }
                else if (s[0] == '(')
                {
                    connectives.Add('(');
                }
                else if (s[0] == ')')
                {
                    connectives.RemoveAt(connectives.Count - 1);
                    char currentOperator = connectives[connectives.Count - 1];
                    connectives.RemoveAt(connectives.Count - 1);
                    CreateConnective(currentOperator);
                }
                ParseHelper(s.Substring(1));
            }
        }

        private void CreateConnective(char connective)
        {
            Proposition result = null;
            switch (connective)
            {
                case '~':
                    result = new Negation();
                    break;
                case '>':
                    result = new Implication();
                    break;
                case '=':
                    result = new BiImplication();
                    break;
                case '&':
                    result = new Conjunction();
                    break;
                case '|':
                    result = new Disjunction();
                    break;
            }
            if (result is BinaryConnective)
            {
                ((BinaryConnective)result).LeftSuccessor = symbols[symbols.Count - 2];
                ((BinaryConnective)result).RightSuccessor = symbols[symbols.Count - 1];
                symbols.RemoveAt(symbols.Count - 1);
                symbols.RemoveAt(symbols.Count - 1);
            }
            else
            {
                ((UnaryConnective)result).LeftSuccessor = symbols[symbols.Count - 1];
                symbols.RemoveAt(symbols.Count - 1);
            }
            symbols.Add(result);
        }

        /// <summary>
        /// Checks if the variable by character is already in the expression.
        /// 
        /// If it is, this method returns the reference to that already existing
        /// symbol, if it does not, it will return a null value.
        /// </summary>
        /// <param name="variable">A character representing an abstract proposition variable</param>
        /// <returns>A symbol if there already exists a variable with that identifier</returns>
        private Proposition IsVariableInExpression(char variable)
        {
            foreach (Proposition processedVariable in alreadyProcessedVariables)
            {
                if ((char)processedVariable.Data == variable)
                {
                    return processedVariable;
                }
            }
            return null;
        }
    }
}
