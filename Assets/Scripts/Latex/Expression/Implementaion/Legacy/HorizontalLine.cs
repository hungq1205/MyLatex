using System.Text;
using UnityEngine;

namespace Latex
{
    public class HorizontalLine : ExpressionBase
    {
        const char HorizontalDash = '\u2500';

        public float thickness, lpad, rpad, yOffset;
        IExpression boundTracer;

        public HorizontalLine(IExpression boundTracer, float thickness, float yOffset = 0f, float lpad = 0f, float rpad = 0f) : base()
        {
            this.thickness = thickness;
            this.lpad = lpad;
            this.rpad = rpad;
            this.yOffset = yOffset;
            this.boundTracer = boundTracer;
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

            float lBound = boundTracer.TopLeft.x - lpad;
            float rBound = boundTracer.BottomRight.x + rpad;

            var vertIdx = latex.tInfo.characterInfo[StartChar].vertexIndex;
            var vertices = latex.tInfo.meshInfo[latex.tInfo.characterInfo[StartChar].materialReferenceIndex].vertices;

            vertices[vertIdx] = new Vector2(lBound, vertices[vertIdx].y + yOffset);
            vertices[vertIdx + 1] = new Vector2(lBound, vertices[vertIdx + 1].y + yOffset);
            vertices[vertIdx + 2] = new Vector2(rBound, vertices[vertIdx + 2].y + yOffset);
            vertices[vertIdx + 3] = new Vector2(rBound, vertices[vertIdx + 3].y + yOffset);

            RenderEnd(latex, preceeding);
        }

        public void Render(Latex latex, float start, float end, IExpression preceeding = null)
        {
            RenderStart(latex, preceeding);

            float lBound = start;
            float rBound = end;

            var vertIdx = latex.tInfo.characterInfo[StartChar].vertexIndex;
            var vertices = latex.tInfo.meshInfo[latex.tInfo.characterInfo[StartChar].materialReferenceIndex].vertices;

            vertices[vertIdx] = new Vector2(lBound, vertices[vertIdx].y + yOffset);
            vertices[vertIdx + 1] = new Vector2(lBound, vertices[vertIdx + 1].y + yOffset);
            vertices[vertIdx + 2] = new Vector2(rBound, vertices[vertIdx + 2].y + yOffset);
            vertices[vertIdx + 3] = new Vector2(rBound, vertices[vertIdx + 3].y + yOffset);

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
