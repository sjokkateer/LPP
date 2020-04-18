﻿using System.Collections.Generic;
using System.Linq;
using System;

namespace LogicAndSetTheoryApplication
{
    public class Parser
    {
        private const string CONNECTIVES = "~>=&|";
        private const string VARIABLES = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string CONSTANTS = "01";

        private string proposition;

        // New addition to keep track of the variables that have already been inserted.
        private List<Proposition> alreadyProcessedVariables;

        private List<char> connectives;
        private List<Proposition> symbols;

        // Proposition has to be given in pre-fix notation.
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
                else if (CONSTANTS.Contains(s[0]))
                {
                    Proposition symbol;
                    
                    if (s[0] == '0')
                    {
                        symbol = new False();
                    }
                    else
                    {
                        symbol = new True();
                    }

                    symbols.Add(symbol);
                }
                else if (s[0] == '(')
                {
                    connectives.Add('(');
                }
                else if (s[0] == ')')
                {
                    // If we reach a closing parenth we should check if we have an opening
                    // parenth already, otherwise we could consider the expression invalid.

                    // There should be equal pairs of opening and closing parenths or the 
                    // expression could be considered invalid.

                    // Tries to remove the corresponding opening parenthesis.
                    connectives.RemoveAt(connectives.Count - 1);

                    // Pops the operator that preceeded the opening parenth.
                    char currentOperator = connectives[connectives.Count - 1];
                    connectives.RemoveAt(connectives.Count - 1);

                    // Creates a connective from it.
                    CreateConnective(currentOperator);
                }

                ParseHelper(s.Substring(1));
            }
        }

        private void CreateConnective(char connective)
        {
            Proposition result = null;

            // 'GetCorrespondingConnective'
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

            // 'CreateExpression'
            // The symbols[symbols.Count - 1] can always be done. Placement is dependent on Unary or Binary connective.
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

            // Now the expression is a symbol in case a nested connective needs to use it
            // as right or left successor.
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
