using System.Text;
using UnityEngine;

namespace Latex
{
    public class TextExpression : ExpressionBase
    {
        public string value;

        public TextExpression(string content) : base()
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

            vertIdx = latex.tInfo.characterInfo[StartChar + Length - 1].vertexIndex;
            vertices = latex.tInfo.meshInfo[latex.tInfo.characterInfo[StartChar + Length - 1].materialReferenceIndex].vertices;
            bottomRight = vertices[vertIdx + 3];
        }
    }
}