using UnityEngine;
using TMPro;
using System.Text;
using UnityEngine.UIElements;

namespace Latex
{
    public abstract class ExpressionBase : IExpression
    {
        public IExpression[] Content { get; protected set; }
        public bool Isolated { get; private set; }
        public int StartChar { get; protected set; }
        public int Length { get; protected set; }

        protected Vector2 topLeft;
        public Vector2 TopLeft => topLeft;

        protected Vector2 bottomRight;
        public Vector2 BottomRight => bottomRight;

        public ExpressionBase(bool isolated = false) 
        {
            Isolated = isolated;
        }

        public virtual void Build(StringBuilder sb)
        {
            StartChar = sb.Length;
            for (int i = 0; i < Content.Length; i++)
                Content[i].Build(sb);
            Length = sb.Length - StartChar;
        }

        public virtual void Render(Latex latex)
        {
            if (StartChar < 0 || Length < 0)
                throw new System.Exception("Expression has not been built");
        }

        public virtual void Transform(Latex latex, float scale)
        {
            if (Content != null)
            {
                Vector2 anchor = new(Content[0].TopLeft.x, Content[0].BottomRight.y);
                for (int i = 0; i < Content.Length; i++)
                {
                    Content[i].Transform(latex, scale, anchor);
                    if (!Isolated)
                    {
                        anchor = Content[i].BottomRight;
                        anchor.x += latex.spacing;
                    }
                }
            }
            else
                Transform(latex, scale, StartChar, Length);
            UpdateBound(latex);
        }

        public virtual void Transform(Latex latex, float scale, Vector2 anchor)
        {
            if (Content != null)
            {
                for (int i = 0; i < Content.Length; i++)
                {
                    Content[i].Transform(latex, scale, anchor);
                    if (!Isolated)
                    {
                        anchor = Content[i].BottomRight;
                        anchor.x += latex.spacing;
                    }
                }
            }
            else
                Transform(latex, scale, StartChar, Length, anchor);
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

                for (int i = 0; i < 4; i++)
                    vertices[vertIdx + i] = (vertices[vertIdx + i] - cInfo.bottomLeft) * scale + anc;
                anc = cInfo.bottomRight;
                anc.x += latex.spacing;
            }
        }

        static void Transform(Latex latex, float scale, int startChar, int length, Vector2 anchor)
        {
            TMP_TextInfo tInfo = latex.tInfo;
            TMP_CharacterInfo cInfo;
            Vector3 anc = new(anchor.x, anchor.y);
            Vector3[] vertices;
            int vertIdx;

            for (int c = startChar; c < startChar + length; c++)
            {
                cInfo = tInfo.characterInfo[c];
                vertIdx = cInfo.vertexIndex;
                vertices = tInfo.meshInfo[cInfo.materialReferenceIndex].vertices;

                for (int i = 0; i < 4; i++)
                    vertices[vertIdx + i] = (vertices[vertIdx + i] - cInfo.bottomLeft) * scale + anc;
                anc = cInfo.bottomRight;
                anc.x += latex.spacing;
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