using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicAndSetTheoryApplication
{
    public class LogicApp: LogicAppBase
    {
        private const int HASH_CODE_BASE = 16;

        private HashCodeCalculator hashCodeCalculator;
        
        private List<string> hashCodes;
        public static readonly List<string> hashCodeLabels = new List<string>()
        { 
            "Original",
            "Simplified",
            "Nand Simplified",
            "Original DNF",
            "Original Simplified DNF",
            "Original Simplified DNF Nand"
        };
        public List<string> HashCodes
        {
            get
            {
                // return a copy of the original list.
                return new List<string>(hashCodes);
            }
        }

        public List<Proposition> Variables
        {
            get
            {
                if (Root == null)
                {
                    throw new NullReferenceException("Need a proposition expression first to obtain variables!");
                }

                return Root.GetVariables();
            }
        }
        public TruthTable TruthTable { get; private set; }
        public Proposition DisjunctiveNormalForm { get; private set; }
        public TruthTable SimplifiedTruthTable { get; private set; }
        public Proposition Nandified { get; private set; }

        public LogicApp(Parser parser): base(parser)
        {
            hashCodeCalculator = new HashCodeCalculator(HASH_CODE_BASE);
        }

        protected override void ExecuteParsingActivities()
        { 
            // New list of all the required hashcodes.
            hashCodes = new List<string>();

            // 1
            TruthTable = new TruthTable(Root);
            hashCodeCalculator.GenerateHashCode(TruthTable.GetConvertedResultColumn());
            hashCodes.Add(hashCodeCalculator.HashCode);

            // 2
            SimplifiedTruthTable = TruthTable.Simplify();

            if (NonConstantExpressionWasParsed())
            {
                // Convert the table to DNF such that we have a proposition to work with.
                Proposition simplified = SimplifiedTruthTable.CreateDisjunctiveNormalForm();
                TruthTable simplifiedTt = new TruthTable(simplified);
                // Check the hash code for the proposition.
                hashCodeCalculator.GenerateHashCode(simplifiedTt.GetConvertedResultColumn());
                hashCodes.Add(hashCodeCalculator.HashCode);

                // 3 
                Nandified = Root.Nandify();
                TruthTable nandifiedTruthTable = new TruthTable(Nandified);
                TruthTable nandifiedSimplified = nandifiedTruthTable.Simplify();

                // Same as for simplified truth table.
                Proposition nandSimpleDnf = nandifiedSimplified.CreateDisjunctiveNormalForm();
                TruthTable nandSimpleDnfTt = new TruthTable(nandSimpleDnf);

                hashCodeCalculator.GenerateHashCode(nandSimpleDnfTt.GetConvertedResultColumn());
                hashCodes.Add(hashCodeCalculator.HashCode);

                // 4 
                // DisjunctiveNormalForm = CreateDisjunctiveNormalForm();
                DisjunctiveNormalForm = TruthTable.CreateDisjunctiveNormalForm();
                TruthTable disjunctiveTruthTable = new TruthTable(DisjunctiveNormalForm);
                hashCodeCalculator.GenerateHashCode(disjunctiveTruthTable.GetConvertedResultColumn());
                hashCodes.Add(hashCodeCalculator.HashCode);

                // 5
                Proposition simplifiedDisjunctiveNormal = SimplifiedTruthTable.CreateDisjunctiveNormalForm();
                TruthTable simplifiedDisjunctiveNormalTruthTable = new TruthTable(simplifiedDisjunctiveNormal);
                hashCodeCalculator.GenerateHashCode(simplifiedDisjunctiveNormalTruthTable.GetConvertedResultColumn());
                hashCodes.Add(hashCodeCalculator.HashCode);

                // 6
                Proposition nandifiedSimplifiedDisjunctiveNormal = simplifiedDisjunctiveNormal.Nandify();
                TruthTable nandifiedSimplifiedDisjunctiveNormalTruthTable = new TruthTable(nandifiedSimplifiedDisjunctiveNormal);
                hashCodeCalculator.GenerateHashCode(nandifiedSimplifiedDisjunctiveNormalTruthTable.GetConvertedResultColumn());
                hashCodes.Add(hashCodeCalculator.HashCode);
            }
        }
        
        public bool NonConstantExpressionWasParsed()
        {
            return Root.GetVariables().Count > 0;
        }

        public bool HashCodesMatched()
        {
            return HashCodesMatched(HashCodes, HashCodes.Count - 1);
        }
       
        private bool HashCodesMatched(List<string> hashList, int index)
        {
            if (index == 0)
            {
                return true;
            }
            else
            {
                // Compare the two adjacent hashcodes and make a call downwards.
                return hashList[index] == hashList[index - 1] && HashCodesMatched(hashList, index - 1);
            }
        }

        [ExcludeFromCodeCoverage]
        protected override string DotFileName()
        {
            return "Proposition";
        }

        [ExcludeFromCodeCoverage]
        public override void CreateGraphImage()
        {
            Grapher.CreateGraphOfProposition(Root.Copy(), DotFileName());
            base.CreateGraphImage();
        }
    }
}
