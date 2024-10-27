using UnityEngine;

namespace Latex
{
    [LatexExpression("\\frac")]
    public class Fraction : ExpressionBase
    {
        const float ExpressionScaler = 0.75f;
        const float ExpressionSpacing = 12f;
        const float ExpressionOffset = 1.5f;
        const float DividerAnchorCoef = 0.6f;
        const float Pad = 12f;

        public Fraction(params IExpression[] content) : base()
        {
            if (content.Length != 2)
                throw new System.Exception("Invalid expression numbers for " + GetType());

            SpacingLeft += 5f;
            SpacingRight += 5f;
            Content = new IExpression[] {
                content[0], 
                content[1],
                new HorizontalLine(1f, Pad, Pad, content[0], content[1]),
            };
        }

        public override void Render(Latex latex, IExpression preceeding = null)
        {
            RenderStart(latex, preceeding);

            Vector2 anchor;
            if (preceeding == null)
                anchor = Content[0].Position;
            else
            {
                anchor = Content[0].Position;
                anchor.x = preceeding.BottomRight.x;
            }

            Content[0].Render(latex);
            Content[0].Transform(latex, ExpressionScaler, new Vector2(anchor.x, anchor.y + ExpressionSpacing - ExpressionOffset), 0);

            Content[1].Render(latex);
            Content[1].Transform(latex, ExpressionScaler, new Vector2(anchor.x, anchor.y - ExpressionSpacing - ExpressionOffset), 1);

            Content[2].Render(latex);

            RenderEnd(latex, preceeding);
        }
    }
}
