using System.Text;

namespace Latex
{
    public class CharExpression : ExpressionBase
    {
        public char value;

        public CharExpression(char content) : base()
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
            int vertIdx = latex.tInfo.characterInfo[StartChar].vertexIndex;
            var vertices = latex.tInfo.meshInfo[latex.tInfo.characterInfo[StartChar].materialReferenceIndex].vertices;

            topLeft = vertices[vertIdx + 1];
            bottomRight = vertices[vertIdx + 3];
        }
    }
}