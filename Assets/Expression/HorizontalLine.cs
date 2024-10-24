using System.Text;
using UnityEngine;

namespace Latex
{
    public class HorizontalLine : ExpressionBase
    {
        const char HorizontalDash = '\u2500';

        public float thickness;
        IExpression[] lengthTrailing;

        public HorizontalLine(float thickness, params IExpression[] lengthTrailing) : base()
        {
            this.thickness = thickness;
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
            base.Render(latex, preceeding);

            float lBound = float.PositiveInfinity, rBound = float.NegativeInfinity;
            foreach (var ep in lengthTrailing)
            {
                if (ep.BottomRight.x > rBound) rBound = ep.BottomRight.x;
                if (ep.TopLeft.x < lBound) lBound = ep.TopLeft.x;
            }

            var vertIdx = latex.tInfo.characterInfo[StartChar].vertexIndex;
            var vertices = latex.tInfo.meshInfo[latex.tInfo.characterInfo[StartChar].materialReferenceIndex].vertices;

            vertices[vertIdx].x     = lBound;
            vertices[vertIdx + 1].x = lBound;
            vertices[vertIdx + 2].x = rBound;
            vertices[vertIdx + 3].x = rBound;
        }

        public override void Transform(Latex latex, float scale, Vector2 pos, float anchorCoef = 0f)
        {
            var cInfo = latex.tInfo.characterInfo[StartChar];
            var vertIdx = cInfo.vertexIndex;
            var vertices = latex.tInfo.meshInfo[cInfo.materialReferenceIndex].vertices;

            var pos3 = new Vector3(pos.x, pos.y - (vertices[vertIdx + 1].y - vertices[vertIdx + 3].y) * scale * anchorCoef);
            var oldAnchor = vertices[vertIdx];
            oldAnchor.y += (vertices[vertIdx + 1].y - vertices[vertIdx + 3].y) * anchorCoef;

            for (int i = 0; i < 4; i++)
                vertices[vertIdx + i] = (vertices[vertIdx + i] - oldAnchor) * scale + pos3;
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
