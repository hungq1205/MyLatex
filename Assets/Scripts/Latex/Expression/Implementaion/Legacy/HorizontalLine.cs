using System.Text;
using UnityEngine;

namespace Latex
{
    public class HorizontalLine : ExpressionBase
    {
        const char HorizontalDash = '\u2500';

        public float thickness, lpad, rpad;
        IExpression[] lengthTrailing;

        public HorizontalLine(float thickness, float lpad = 0f, float rpad = 0f, params IExpression[] lengthTrailing) : base()
        {
            this.thickness = thickness;
            this.lpad = lpad;
            this.rpad = rpad;
            this.lengthTrailing = lengthTrailing;
        }

        public override void Build(StringBuilder sb)
        {
            StartChar = sb.Length;
            sb.Append(HorizontalDash);
            Length = sb.Length - StartChar;
        }

        public override void Render(Latex latex, IExpression preceeding = null)
        {
            RenderStart(latex, preceeding);

            float lBound = float.PositiveInfinity, rBound = float.NegativeInfinity;
            foreach (var ep in lengthTrailing)
            {
                if (ep.BottomRight.x > rBound) rBound = ep.BottomRight.x;
                if (ep.TopLeft.x < lBound) lBound = ep.TopLeft.x;
            }
            lBound = lBound - lpad;
            rBound = rBound + rpad;

            var vertIdx = latex.tInfo.characterInfo[StartChar].vertexIndex;
            var vertices = latex.tInfo.meshInfo[latex.tInfo.characterInfo[StartChar].materialReferenceIndex].vertices;

            vertices[vertIdx].x     = lBound;
            vertices[vertIdx + 1].x = lBound;
            vertices[vertIdx + 2].x = rBound;
            vertices[vertIdx + 3].x = rBound;

            RenderEnd(latex, preceeding);
        }

        public override void UpdateBound(Latex latex)
        {
            var cInfo = latex.tInfo.characterInfo[StartChar];
            var vertIdx = cInfo.vertexIndex;
            var vertices = latex.tInfo.meshInfo[cInfo.materialReferenceIndex].vertices;

            topLeft = vertices[vertIdx + 1];
            bottomRight = vertices[vertIdx + 3];
        }
    }
}
