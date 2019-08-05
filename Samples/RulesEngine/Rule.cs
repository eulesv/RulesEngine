using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;

namespace RulesEngine
{
    public class Rule
    {
        public string Member
        {
            get;
            set;
        }

        public string Operation
        {
            get;
            set;
        }

        public object Target
        {
            get;
            set;
        }

        public Rule Constraint
        {
            get;
            set;
        }

    }
}
