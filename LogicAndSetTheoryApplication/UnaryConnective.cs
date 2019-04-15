﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicAndSetTheoryApplication
{
    class UnaryConnective : Symbol
    {
        public Symbol LeftSuccessor { get; set; }

        public UnaryConnective(object data) : base(data)
        {
            LeftSuccessor = null;
        }

        public override List<Symbol> GetVariables()
        {
            return LeftSuccessor.GetVariables().Distinct().ToList();
        }

        public override string ToString()
        {
            string result = Convert.ToString(Data);
            if (!(LeftSuccessor is BinaryConnective))
            {
                result += "(";
                result += LeftSuccessor;
                result += ")";
            }
            else
            {
                result += LeftSuccessor;
            }
            return result;
        }

        public override string NodeLabel()
        {
            string result = base.NodeLabel();
            result += $"\tnode{NodeNumber} -- node{LeftSuccessor.NodeNumber}\n";
            result += LeftSuccessor.NodeLabel();
            return result;
        }
    }
}
