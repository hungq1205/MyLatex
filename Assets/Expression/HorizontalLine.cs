using System.Text;
using TMPro;
using UnityEngine;

namespace Latex
{
    public class HorizontalLine : ExpressionBase
    {
        const char HorizontalDash = '\u2500';

        public float thickness, padding;
        IExpression[] lengthTrailing;

        public HorizontalLine(
            float thickness, 
            float padding = 0, 
            bool isolated = false, 
            params IExpression[] lengthTrailing) : base(isolated)
        {
            this.thickness = thickness;
            this.padding = padding;
            this.lengthTrailing = lengthTrailing;
        }

        public override void Build(StringBuilder sb)
        {
            StartChar = sb.Length;
            sb.Append(HorizontalDash);
            Length = sb.Length - StartChar;
        }

        public override void Transform(Latex latex, float scale, Vector2 anchor)
        {
            var cInfo = latex.tInfo.characterInfo[StartChar];
            var vertIdx = cInfo.vertexIndex;
            var vertices = latex.tInfo.meshInfo[cInfo.materialReferenceIndex].vertices;
            var anc = new Vector3(anchor.x, anchor.y);

            for (int i = 0; i < 4; i++)
                vertices[vertIdx + i] = (vertices[vertIdx + i] - cInfo.bottomLeft) * scale + anc;
        }
    }
}
