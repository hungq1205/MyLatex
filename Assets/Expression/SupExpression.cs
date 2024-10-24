namespace Latex
{
    public class SupExpression : ExpressionBase
    {
        const float scaler = 0.4f;

        public SupExpression(IExpression content) : base()
        {
            Content = new []{ content };
        }

        public override void Render(Latex latex, IExpression preceeding = null)
        {
            base.Render(latex, preceeding);

            Content[0].Render(latex);
            Content[0].Transform(latex, scaler, preceeding.TopRight, 0.5f);
        }
    }
}