﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicAndSetTheoryApplication
{
    class Proposition : IComparable<Proposition>
    {
        public int NodeNumber { get; set; }
        public object Data { get; }
        public bool TruthValue { get; set; }
        public Proposition(char data)
        {
            Data = data;
        }

        // Probably can remove the IEquatable interface since we can compare references now.
        public virtual List<Proposition> GetVariables()
        {
            return new List<Proposition>() { this };
        }

        public virtual bool Calculate()
        {
            return TruthValue;
        }

        public override string ToString()
        {
            return Data.ToString();
        }

        public virtual string NodeLabel()
        {
            return $"\tnode{NodeNumber} [ label = \"{Data}\" ]\n";
        }

        public int CompareTo(Proposition other)
        {
            char ownVariable = (char)Data;
            char otherVariable = (char)other.Data;

            return ownVariable.CompareTo(otherVariable);
        }
        public virtual Proposition Copy()
        {
            return new Proposition((char)Data);
        }
    }
}