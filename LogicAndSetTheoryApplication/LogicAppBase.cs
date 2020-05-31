﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

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

        abstract protected string DotFileName();

        public virtual void CreateGraphImage()
        {
            Process.Start($"{DotFileName()}.png");
        }

        abstract protected void ExecuteParsingActivities();

        public void Parse(string propositionExpression)
        {
            if (!propositionExpression.Equals(parsedExpression))
            {
                Root = parser.Parse(propositionExpression);
                ExecuteParsingActivities();
                parsedExpression = propositionExpression;
            }
        }
    }
}
