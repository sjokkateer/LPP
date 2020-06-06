﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicAndSetTheoryApplication
{
    public class Proposition : IComparable<Proposition>
    {
        [ExcludeFromCodeCoverage]
        public int NodeNumber { get; set; }
        [ExcludeFromCodeCoverage]
        public char Data { get; }
        [ExcludeFromCodeCoverage]
        public bool TruthValue { get; set; }

        public Proposition() { }

        public Proposition(char data)
        {
            Data = data;
        }

        [ExcludeFromCodeCoverage]
        public List<Proposition> UniqueVariableSet { get; set; }

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
            return $"node{NodeNumber} [ label = \"{Data}\" ]\n";
        }

        public virtual bool IsAbstractProposition()
        {
            return true;
        }

        public override int GetHashCode()
        {
            return Data.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            // We dont want to compare by reference
            // only by connectives and variables.
            if (obj == null) return false;
            if (GetType() != obj.GetType()) return false;

            Proposition proposition = (Proposition)obj;
            return Data.Equals(proposition.Data);
        }

        public int CompareTo(Proposition other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("The other proposition must not be null!");
            }

            char ownVariable = Data;
            char otherVariable = other.Data;

            return ownVariable.CompareTo(otherVariable);
        }

        public virtual Proposition Nandify()
        {
            // A == ~(~(A))
            // ~(A) == ~(A ^ A) == A % A
            // ~(~(A ^ A)) == ~(A % A)
            // == (A % A) % (A % A)
            Nand nand1 = new Nand();
            Nand nand2 = new Nand();
            Nand nand3 = new Nand();
            
            nand2.LeftSuccessor = this;
            nand2.RightSuccessor = this;
            
            nand3.LeftSuccessor = this;
            nand3.RightSuccessor = this;

            nand1.LeftSuccessor = nand2;
            nand1.RightSuccessor = nand3;
            
            return nand1;
        }

        public virtual Proposition Copy()
        {
            return new Proposition(Data);
        }

        public virtual bool Replace(char originalVariable, char newVariable)
        {
            throw new NotImplementedException("Can not apply to abstract proposition variable");
        }
    }
}