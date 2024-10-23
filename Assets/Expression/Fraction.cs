using System.Text;
using UnityEngine;

namespace Latex
{
    public class Fraction : ExpressionBase
    {
        const float ExpressionScaler = 0.85f;
        const float ExpressionOffset = 35f;
        const float DividerAnchorCoef = 0.3f;

        public Fraction(IExpression numerator, IExpression denominator) : base()
        {
            Content = new IExpression[] { 
                numerator, 
                denominator,
                new HorizontalLine(1f, numerator, denominator),
            };
        }

        public override void Render(Latex latex, IExpression preceeding = null)
        {
            base.Render(latex, preceeding);

            Vector2 anchor;
            if (preceeding == null)
                anchor = Content[0].BottomLeft;
            else
            {
                anchor = Content[0].BottomLeft;
                anchor.x = preceeding.BottomRight.x;
            }

            Content[0].Render(latex);
            Content[0].Transform(latex, ExpressionScaler, new Vector2(anchor.x, anchor.y + ExpressionOffset), DividerAnchorCoef);

            Content[1].Render(latex);
            Content[1].Transform(latex, ExpressionScaler, new Vector2(anchor.x, anchor.y - ExpressionOffset), DividerAnchorCoef);

            Content[2].Render(latex);

            UpdateBound(latex);
        }
    }
}
