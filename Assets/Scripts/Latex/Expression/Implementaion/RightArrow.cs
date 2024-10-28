namespace Latex
{
    [LatexExpression("\\rightarrow")]
    public class RightArrow : ExpressionBase
    {
        const char Symbol = '\u2192';
        const float SymbolScaler = 1.4f;
        const float yOffset = -4.1f;
        const float UpperScaler = 0.45f;

        public float length;

        public RightArrow(params IExpression[] content) : base()
        {
            if (content.Length > 1)
                throw new System.Exception("Invalid expression numbers for " + GetType());

            if (content.Length == 0)
            {
                Content = new IExpression[] {
                    new HorizontalLine(null, 10f, yOffset),
                    new CharExpression(Symbol),
                };
            }
            else
            {
                Content = new IExpression[] {
                    new HorizontalLine(null, 10f, yOffset),
                    new CharExpression(Symbol),
                    content[0],
                };
            }
            Content[0].SpacingRight = 0f;
            Content[1].SpacingLeft = -4.25f;
        }

        public override void Render(Latex latex, IExpression preceeding = null)
        {
            RenderStart(latex, preceeding);

            float start = preceeding == null ? TopLeft.x : preceeding.BottomRight.x + preceeding.SpacingRight + SpacingLeft;
            ((HorizontalLine)Content[0]).Render(latex, start, start + 60f);

            Content[1].Render(latex);
            Content[1].Transform(latex, SymbolScaler, Content[0]);
            
            if (Content.Length > 2)
            {
                Content[2].Render(latex);
                Content[2].Transform(latex, UpperScaler);

                float xPos = 
                    Content[0].TopLeft.x + 
                    (Content[1].BottomRight.x - Content[0].TopLeft.x) * 0.5f - 
                    (Content[2].BottomRight.x - Content[2].TopLeft.x) * 0.5f;
                Content[2].Transform(latex, 1f, new UnityEngine.Vector2(xPos, Content[1].TopLeft.y + 2f));
            }

            RenderEnd(latex, preceeding);
        }
    }
}