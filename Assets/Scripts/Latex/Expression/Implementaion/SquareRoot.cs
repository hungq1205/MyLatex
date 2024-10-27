namespace Latex
{
    [LatexExpression("\\sqrt")]
    public class SquareRoot : ExpressionBase
    {
        const char SqrtChar = '\u221A';
        const float SqrtSymbolScaler = 1.1f;
        const float PadL = 5.8f, PadR = 11f;
        static readonly UnityEngine.Vector2 Offset = new(-PadL, -3f);

        public float rPad, lPad;

        public SquareRoot(params IExpression[] content) : base()
        {
            if (content.Length != 1)
                throw new System.Exception("Invalid expression numbers for " + GetType());

            Content = new IExpression[] {
                new CharExpression(SqrtChar),
                content[0],
                new HorizontalLine(1f, PadL, PadR, content[0]),
            };
        }

        public override void Render(Latex latex, IExpression preceeding = null)
        {
            RenderStart(latex, preceeding);

            Content[0].Render(latex);
            Content[0].Transform(latex, SqrtSymbolScaler, preceeding);

            Content[1].Render(latex);
            Content[1].Transform(latex, 1f, Content[0]);

            Content[2].Render(latex);
            Content[2].Transform(latex, 1f, Content[0].TopRight + Offset, 0.5f);

            RenderEnd(latex, preceeding);
        }
    }
}