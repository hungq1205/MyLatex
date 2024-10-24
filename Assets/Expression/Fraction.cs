using UnityEngine;

namespace Latex
{
    public class Fraction : ExpressionBase
    {
        const float ExpressionScaler = 0.9f;
        const float ExpressionSpacing = 37.5f;
        const float ExpressionOffset = 20.5f;
        const float DividerAnchorCoef = 0.5f;

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
            Content[0].Transform(latex, ExpressionScaler, new Vector2(anchor.x, anchor.y + ExpressionSpacing + ExpressionOffset), DividerAnchorCoef);

            Content[1].Render(latex);
            Content[1].Transform(latex, ExpressionScaler, new Vector2(anchor.x, anchor.y - ExpressionSpacing + ExpressionOffset), DividerAnchorCoef);

            Content[2].Render(latex);

            UpdateBound(latex);
        }
    }
}
