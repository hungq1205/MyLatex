using System;
using System.Collections.Generic;
using System.Reflection;

namespace Latex
{
    [AttributeUsage(AttributeTargets.Class)]
    public class LatexExpressionAttribute : Attribute
    {
        public string LatexID { get; private set; }

        public LatexExpressionAttribute(string latexID)
        {
            LatexID = latexID;
        }
    }
}
