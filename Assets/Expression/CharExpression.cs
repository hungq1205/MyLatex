using System.Text;

namespace Latex
{
    public class CharExpression : ExpressionBase
    {
        public char value;

        public CharExpression(char content, bool isolated = false) : base(isolated)
        {
            value = content;
        }

        public override void Build(StringBuilder sb)
        {
            StartChar = sb.Length;
            sb.Append(value);
            Length = sb.Length - StartChar;
        }

        public override void UpdateBound(Latex latex)
        {
            topLeft = latex.tInfo.characterInfo[StartChar].topLeft;
            bottomRight = latex.tInfo.characterInfo[StartChar].bottomRight;
        }
    }
}