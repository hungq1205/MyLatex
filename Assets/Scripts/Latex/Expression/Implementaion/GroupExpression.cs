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
            RenderStart(latex, preceeding);

            var temp = preceeding;
            float spacing = -Content[0].SpacingLeft;
            for (int i = 0; i < Content.Length; i++)
            {
                spacing += Content[i].SpacingLeft;
                Content[i].Render(latex, temp);
                Content[i].Transform(latex, 1f, Content[i].Position + UnityEngine.Vector2.right * spacing);
                UnityEngine.Debug.Log(Content[i].GetType() + ": " + Content[i].TopLeft);
                UnityEngine.Debug.Log(Content[i].GetType() + ": " + Content[i].BottomRight);
                temp = Content[i];
                spacing = Content[i].SpacingRight;
            }

            RenderEnd(latex, preceeding);
        }
    }
}
