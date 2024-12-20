using UnityEngine;

namespace Latex
{
    [LatexExpression("\\frac")]
    public class Fraction : ExpressionBase
    {
        const float ExpressionScaler = 0.75f;
        const float ExpressionSpacing = 12f;
        const float ExpressionOffset = 3f;
        const float DividerOffsetY = 1f;
        const float Pad = 18f;

        BoundAggregator boundTracer;

        public Fraction(params IExpression[] content) : base()
        {
            if (content.Length != 2)
                throw new System.Exception("Invalid expression numbers for " + GetType());

            boundTracer = new BoundAggregator(true, content[0], content[1]);
            Content = new IExpression[] {
                content[0],
                content[1],
                new HorizontalLine(boundTracer, 1f, DividerOffsetY, Pad, Pad),
            };
            SpacingLeft += 5f;
            SpacingRight += 5f;
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
                anchor.x = preceeding.BottomRight.x + preceeding.SpacingRight + SpacingLeft;
            }

            Content[0].Render(latex);
            Content[0].Transform(latex, ExpressionScaler, new Vector2(anchor.x, anchor.y + ExpressionSpacing - ExpressionOffset), 0);

            Content[1].Render(latex);
            Content[1].Transform(latex, ExpressionScaler, new Vector2(anchor.x, anchor.y - ExpressionSpacing - ExpressionOffset), 1);

            boundTracer.Render(latex);
            Content[2].Render(latex, preceeding);

            HorizontalAlign(latex, Content[0], boundTracer);
            HorizontalAlign(latex, Content[1], boundTracer);

            RenderEnd(latex, preceeding);
        }
    }
}
