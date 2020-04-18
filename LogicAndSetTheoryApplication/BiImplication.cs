using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicAndSetTheoryApplication
{
    public class BiImplication : BinaryConnective
    {
        public BiImplication() : base('=')
        { }

        public override bool Calculate()
        {
            // Will be true if both left and right successor are true or false
            return  LeftSuccessor.Calculate() == RightSuccessor.Calculate();
        }

        public override Proposition Copy()
        {
            BiImplication copy = new BiImplication();
            copy.LeftSuccessor = LeftSuccessor.Copy();
            copy.RightSuccessor = RightSuccessor.Copy();
            return copy;
        }

        public override Proposition Nandify()
        {
            Disjunction disjunction = new Disjunction();
            Conjunction leftDisjunctionConjunct = new Conjunction();
            Conjunction rightDisjunctionConjunct = new Conjunction();
            Negation negationOfLeftSuccessor = new Negation();
            negationOfLeftSuccessor.LeftSuccessor = LeftSuccessor;
            Negation negationOfRighSuccessor = new Negation();
            negationOfRighSuccessor.LeftSuccessor = RightSuccessor;
            leftDisjunctionConjunct.LeftSuccessor = negationOfLeftSuccessor;
            leftDisjunctionConjunct.RightSuccessor = negationOfRighSuccessor;
            rightDisjunctionConjunct.LeftSuccessor = LeftSuccessor;
            rightDisjunctionConjunct.RightSuccessor = RightSuccessor;
            disjunction.LeftSuccessor = leftDisjunctionConjunct;
            disjunction.RightSuccessor = rightDisjunctionConjunct;
            return disjunction.Nandify();
        }
    }
}
