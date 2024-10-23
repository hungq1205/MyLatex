using UnityEngine;
using TMPro;

namespace Latex
{
    public class SquareRoot : ExpressionBase
    {
        const char SqrtChar = '\u221A';
        const float SqrtSymbolScaler = 1.1f;

        public float rPad, lPad;

        public SquareRoot(IExpression content) : base()
        {
            Content = new IExpression[] {
                new CharExpression(SqrtChar),
                content,
            };
        }

        public SquareRoot(string content) : base()
        {
            Content = new IExpression[] {
                new CharExpression(SqrtChar),
                new TextExpression(content),
            };
        }

        public override void Render(Latex latex, IExpression preceeding = null)
        {
            base.Render(latex, preceeding);

            Content[0].Render(latex);
            Content[0].Transform(latex, SqrtSymbolScaler, preceeding);

            Content[1].Render(latex);
            Content[1].Transform(latex, 1, Content[0]);

            UpdateBound(latex);
        }
    }
}