namespace Latex
{
    [LatexExpression("\\group")]
    public class GroupExpression : ExpressionBase
    {
        public GroupExpression(params IExpression[] content) : base()
        {
            Content = content;
            SpacingLeft = Content[0].SpacingLeft;
            SpacingRight = Content[Content.Length - 1].SpacingRight;
        }

        public override void Render(Latex latex, IExpression preceeding = null)
        {
            Baseline = latex.tInfo.characterInfo[StartChar].baseLine;

            var temp = preceeding;
            for (int i = 0; i < Content.Length; i++)
            {
                Content[i].Render(latex, temp);
                temp = Content[i];
            }

            RenderEnd(latex, preceeding);
        }
    }
}
