using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace LogicAndSetTheoryApplication
{
    abstract public class LogicAppBase
    {
        protected Parser parser;
        protected string parsedExpression;
        // ModifiedRoot will be used for all calculations by
        // both apps.
        protected Proposition ModifiedRoot { get; set; }
        
        // Root will be used for display and is just a copy of the 
        // originally parsed proposition, otherwise it might
        // display for ex. the nandified root in the app.
        public Proposition Root { get; protected set; }

        public LogicAppBase(Parser parser)
        {
            this.parser = parser;
        }

        [ExcludeFromCodeCoverage]
        abstract protected string DotFileName();

        [ExcludeFromCodeCoverage]
        public virtual void CreateGraphImage()
        {
            Process.Start($"{DotFileName()}.png");
        }

        abstract protected void ExecuteParsingActivities();

        public void Parse(string propositionExpression)
        {
            if (propositionExpression == null || propositionExpression == string.Empty)
            {
                throw new ArgumentException("Proposition expression may not be null or empty string");
            }

            if (!propositionExpression.Equals(parsedExpression))
            {

                ModifiedRoot = parser.Parse(propositionExpression);
                Root = ModifiedRoot.Copy();
                ExecuteParsingActivities();
                parsedExpression = propositionExpression;
            }
        }

        public void Parse(Proposition expression)
        {
            if (expression == null)
            {
                throw new ArgumentException("Proposition may not be null");
            }

            if (!expression.Equals(Root))
            {
                ModifiedRoot = expression;
                Root = ModifiedRoot.Copy();
                ExecuteParsingActivities();
            }
        }
    }
}
