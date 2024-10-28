namespace Latex
{
    [LatexExpression("^")]
    public class SupExpression : ExpressionBase
    {
        const float Scaler = 0.5f;

        public IExpression preceeding;

        public SupExpression(params IExpression[] content) : base()
        {
            if (content.Length != 1)
                throw new System.Exception("Invalid expression numbers for " + GetType());

            Content = new []{ content[0] };
            SpacingRight = 0;
            SpacingLeft = 1f;
        }

        public override void Render(Latex latex, IExpression preceeding = null)
        {
            if (preceeding is SubExpression sub)
                preceeding = sub.preceeding;
            this.preceeding = preceeding;

            RenderStart(latex, preceeding);

            Scale = preceeding.Scale * Scaler;
            Content[0].Render(latex);
            Content[0].Transform(latex, Scale, preceeding.TopRight + UnityEngine.Vector2.right * (preceeding.SpacingRight + SpacingLeft), 0.5f);

            RenderEnd(latex, preceeding);
        }
    }
}