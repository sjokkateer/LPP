﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicAndSetTheoryApplication
{
    public class SemanticTableauxApp: LogicAppBase
    {
        public SemanticTableaux SemanticTableaux { get; private set; }

        public SemanticTableauxApp(Parser parser): base(parser)
        { }

        [ExcludeFromCodeCoverage]
        protected override string DotFileName()
        {
            return "Tableaux";
        }

        protected override void ExecuteParsingActivities()
        {
            SemanticTableaux = new SemanticTableaux(Root);
        }

        public bool IsTautology()
        {
            return SemanticTableaux.IsClosed();
        }

        [ExcludeFromCodeCoverage]
        public override void CreateGraphImage()
        {
            Grapher.CreateGraphOfTableaux(SemanticTableaux, DotFileName());
            base.CreateGraphImage();
        }
    }
}
