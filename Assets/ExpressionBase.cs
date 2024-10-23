using UnityEngine;
using TMPro;
using System.Text;
using UnityEngine.UIElements;

namespace Latex
{
    public abstract class ExpressionBase : IExpression
    {
        public IExpression[] Content { get; protected set; }
        public int StartChar { get; protected set; }
        public int Length { get; protected set; }

        protected Vector2 topLeft;
        public Vector2 TopLeft => topLeft;

        protected Vector2 bottomRight;
        public Vector2 BottomRight => bottomRight;

        public ExpressionBase() { }

        public virtual void Build(StringBuilder sb)
        {
            StartChar = sb.Length;
            for (int i = 0; i < Content.Length; i++)
                Content[i].Build(sb);
            Length = sb.Length - StartChar;
        }

        public virtual void Render(Latex latex, IExpression preceeding = null)
        {
            if (StartChar < 0 || Length < 0)
                throw new System.Exception("Expression has not been built");
            UpdateBound(latex);
        }

        public virtual void Transform(Latex latex, float scale, IExpression preceeding = null)
        {
            if (Content != null)
            {
                preceeding ??= Content[0];
                Vector2 anchor = preceeding.BottomLeft;
                for (int i = 0; i < Content.Length; i++)
                {
                    Content[i].Transform(latex, scale, anchor);
                    anchor = Content[i].BottomRight;
                }
            }
            else if (preceeding == null)
                Transform(latex, scale, StartChar, Length);
            else
                Transform(latex, scale, StartChar, Length, preceeding.BottomRight);
            UpdateBound(latex);
        }

        public virtual void Transform(Latex latex, float scale, Vector2 pos, float anchorCoef = 0f)
        {
            if (Content != null)
            {
                for (int i = 0; i < Content.Length; i++)
                {
                    Content[i].Transform(latex, scale, pos, anchorCoef);
                    pos = Content[i].BottomRight;
                }
            }
            else
                Transform(latex, scale, StartChar, Length, pos, anchorCoef);
            UpdateBound(latex);
        }

        static void Transform(Latex latex, float scale, int startChar, int length)
        {
            TMP_TextInfo tInfo = latex.tInfo;
            TMP_CharacterInfo cInfo;
            Vector3 anc = tInfo.characterInfo[0].bottomLeft;
            Vector3[] vertices;
            int vertIdx;

            for (int c = startChar; c < startChar + length; c++)
            {
                cInfo = tInfo.characterInfo[c];
                vertIdx = cInfo.vertexIndex;
                vertices = tInfo.meshInfo[cInfo.materialReferenceIndex].vertices;

                var temp = vertices[vertIdx + 3].x - vertices[vertIdx + 1].x;
                for (int i = 0; i < 4; i++)
                    vertices[vertIdx + i] = (vertices[vertIdx + i] - vertices[vertIdx]) * scale + anc;
                anc.x += (temp + latex.characterSpacing) * scale;
            }
        }

        static void Transform(Latex latex, float scale, int startChar, int length, Vector2 pos, float anchorCoef = 0f)
        {
            TMP_TextInfo tInfo = latex.tInfo;
            var cInfo = tInfo.characterInfo[startChar];
            var vertIdx = cInfo.vertexIndex;
            var vertices = tInfo.meshInfo[cInfo.materialReferenceIndex].vertices;
            var pos3 = new Vector3(pos.x, pos.y - (vertices[vertIdx + 1].y - vertices[vertIdx + 3].y) * scale * anchorCoef);

            var offset = vertices[vertIdx + 3].x - vertices[vertIdx + 1].x;
            var oldAnchor = vertices[vertIdx];
            oldAnchor.y += (vertices[vertIdx + 1].y - vertices[vertIdx + 3].y) * anchorCoef;
            for (int i = 0; i < 4; i++)
                vertices[vertIdx + i] = (vertices[vertIdx + i] - oldAnchor) * scale + pos3;
            pos3.x += (offset + latex.characterSpacing) * scale;
            
            for (int c = startChar + 1; c < startChar + length; c++)
            {
                cInfo = tInfo.characterInfo[c];
                vertIdx = cInfo.vertexIndex;
                vertices = tInfo.meshInfo[cInfo.materialReferenceIndex].vertices;

                offset = vertices[vertIdx + 3].x - vertices[vertIdx + 1].x;
                oldAnchor = vertices[vertIdx];
                oldAnchor.y += (vertices[vertIdx + 1].y - vertices[vertIdx + 3].y) * anchorCoef;
                for (int i = 0; i < 4; i++)
                    vertices[vertIdx + i] = (vertices[vertIdx + i] - oldAnchor) * scale + pos3;
                pos3.x += (offset + latex.characterSpacing) * scale;
            }
        }

        public virtual void UpdateBound(Latex latex)
        {
            topLeft = new Vector2(float.PositiveInfinity, float.NegativeInfinity);
            bottomRight = new Vector2(float.NegativeInfinity, float.PositiveInfinity);

            foreach (IExpression ep in Content)
            {
                if (ep.TopLeft.x < topLeft.x) topLeft.x = ep.TopLeft.x;
                if (ep.TopLeft.y > topLeft.y) topLeft.y = ep.TopLeft.y;
                if (ep.BottomRight.x > bottomRight.x) bottomRight.x = ep.BottomRight.x;
                if (ep.BottomRight.y < bottomRight.y) bottomRight.y = ep.BottomRight.y;
            }
        }
    }
}