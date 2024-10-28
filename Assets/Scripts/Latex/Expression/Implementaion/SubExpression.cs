namespace Latex
{
    [LatexExpression("_")]
    public class SubExpression : ExpressionBase
    {
        const float Scaler = 0.5f;

        public IExpression preceeding;

        public SubExpression(params IExpression[] content) : base()
        {
            if (content.Length != 1)
                throw new System.Exception("Invalid expression numbers for " + GetType());

            Content = new []{ content[0] };
            SpacingRight = 0;
            SpacingLeft = 1f;
        }

        public override void Render(Latex latex, IExpression preceeding = null)
        {
            if (preceeding is SupExpression sup)
                preceeding = sup.preceeding;
            this.preceeding = preceeding;

            RenderStart(latex, preceeding);

            Scale = preceeding.Scale * Scaler;
            Content[0].Render(latex);
            Content[0].Transform(latex, Scale, preceeding.BottomRight + UnityEngine.Vector2.right * (preceeding.SpacingRight + SpacingLeft), 0.5f);

            RenderEnd(latex, preceeding);
        }
    }
}