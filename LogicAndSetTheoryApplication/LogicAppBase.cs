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
                Root = parser.Parse(propositionExpression);
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
                Root = expression;
                ExecuteParsingActivities();
            }
        }
    }
}
