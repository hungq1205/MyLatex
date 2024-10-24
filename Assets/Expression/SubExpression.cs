namespace Latex
{
    public class SubExpression : ExpressionBase
    {
        const float scaler = 0.4f;

        public SubExpression(IExpression content) : base()
        {
            Content = new []{ content };
        }

        public override void Render(Latex latex, IExpression preceeding = null)
        {
            base.Render(latex, preceeding);

            Content[0].Render(latex);
            Content[0].Transform(latex, scaler, preceeding.BottomRight, 0.5f);
        }
    }
}