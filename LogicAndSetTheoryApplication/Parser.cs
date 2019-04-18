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
        private List<Symbol> alreadyProcessedVariables;

        private List<char> connectives;
        private List<Symbol> symbols;

        public Parser(string proposition)
        {
            this.proposition = proposition;

            connectives = new List<char>();
            symbols = new List<Symbol>();
            alreadyProcessedVariables = new List<Symbol>();
        }

        public Symbol Parse()
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
                    // Instead of immediately creating a new symbol,
                    // check if the variable already occurs in a seperate 
                    // list. If it does, use that same reference inside the expression
                    // (at a different location) that allows for easier substitution of 
                    // definitive values.
                    // This way we can still get the unique variables from every subtree by a recursive call but
                    // the references will still be the same.
                    Symbol symbol = IsVariableInExpression(s[0]);
                    if (symbol == null)
                    {
                        // New symbol created, thus we also have to keep track of those by adding them to the list
                        // tracking duplicate variables.
                        symbol = new Symbol(s[0]);
                        alreadyProcessedVariables.Add(symbol);
                    }
                    foreach(Symbol variable in alreadyProcessedVariables)
                    {
                        Console.WriteLine($"{variable}");
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
            Symbol result = null;
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
        private Symbol IsVariableInExpression(char variable)
        {
            foreach (Symbol processedVariable in alreadyProcessedVariables)
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
