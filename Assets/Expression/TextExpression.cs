using System.Text;
using TMPro;

namespace Latex
{
    public class TextExpression : ExpressionBase
    {
        public string value;

        public TextExpression(string content, bool isolated = false) : base(isolated)
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
            bottomRight = latex.tInfo.characterInfo[StartChar + Length - 1].bottomRight;
        }
    }
}
