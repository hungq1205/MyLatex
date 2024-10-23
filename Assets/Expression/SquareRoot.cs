using UnityEngine;
using TMPro;

namespace Latex
{
    public class SquareRoot : ExpressionBase
    {
        const char SqrtChar = '\u221A';

        public SquareRoot(IExpression content, bool isolated = false) : base(isolated)
        {
            Content = new[] { content };
        }

        public SquareRoot(string content) : base()
        {
            Content = new IExpression[] { 
                new CharExpression(SqrtChar),
                new TextExpression(content),
            };
        }

        public override void Render(Latex latex)
        {
            Content[1].Transform(latex, 1 + latex.scaler);
            //Content[1].Transform(latex, 1 - latex.scaler);
        }
    }
}
