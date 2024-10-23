using System.Text;
using UnityEngine;

namespace Latex
{
    public class Fraction : ExpressionBase
    {
        const char SqrtChar = '\u221A';

        public Fraction(IExpression numerator, IExpression denominator, bool isolated = false) : base(isolated)
        {
            Content = new IExpression[] { numerator, denominator };
        }
    }
}
