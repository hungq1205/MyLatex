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

        public float Scale { get; protected set; } = 1f;
        public float SpacingLeft { get; protected set; } = 5f;
        public float SpacingRight { get; protected set; } = 5f;
        public float Baseline { get; protected set; }

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

        protected void RenderStart(Latex latex, IExpression preceeding = null)
        {
            if (StartChar < 0 || Length < 0)
                throw new System.Exception("Expression has not been built");
            Baseline = latex.tInfo.characterInfo[StartChar].baseLine;
        }

        protected void RenderEnd(Latex latex, IExpression preceeding = null)
        {
            UpdateBound(latex);
        }

        public virtual void Render(Latex latex, IExpression preceeding = null)
        {
            RenderStart(latex, preceeding);
            RenderEnd(latex, preceeding);
        }

        public virtual void Transform(Latex latex, float scale, IExpression preceeding = null, IExpression baseEp = null)
        {
            baseEp ??= this;

            Vector2 pos;
            if (preceeding == null)
                pos = new Vector2(TopLeft.x, Baseline);
            else
                pos = new Vector2(preceeding.BottomRight.x, preceeding.Baseline);

            if (Content != null)
            {
                for (int i = 0; i < Content.Length; i++)
                    Content[i].Transform(latex, scale, pos, baseEp);
            }
            else if (preceeding == null)
                TransformText(latex, this, baseEp, scale);
            else
                TransformText(latex, this, baseEp, scale, pos);
            Baseline = pos.y;
            Scale *= scale;
            UpdateBound(latex);
        }

        public virtual void Transform(Latex latex, float scale, Vector2 pos, float anchorCoef, IExpression baseEp = null)
        {
            baseEp ??= this;

            if (Content != null)
            {
                Content[0].Transform(latex, scale, pos, anchorCoef, baseEp);
                for (int i = 1; i < Content.Length; i++)
                    Content[i].Transform(latex, scale, pos, anchorCoef, baseEp);
            }
            else
                TransformText(latex, this, baseEp, scale, pos, anchorCoef);
            Baseline = pos.y - (1 - scale) * (Baseline - pos.y);
            Scale *= scale;
            UpdateBound(latex);
        }

        public virtual void Transform(Latex latex, float scale, Vector2 pos, IExpression baseEp = null)
        {
            baseEp ??= this;
            float anchor = (baseEp.Baseline - baseEp.BottomRight.y) / (baseEp.TopLeft.y - baseEp.BottomRight.y);
            Transform(latex, scale, pos, anchor, baseEp);
        }

        static void TransformText(Latex latex, IExpression ep, IExpression baseEp, float scale)
        {
            var tInfo = latex.tInfo;
            var cInfo = tInfo.characterInfo[ep.StartChar];
            var vertIdx = cInfo.vertexIndex;
            var vertices = tInfo.meshInfo[cInfo.materialReferenceIndex].vertices;
            Vector3 anc = baseEp.Position;

            for (int i = 0; i < 4; i++)
                vertices[vertIdx + i] = (vertices[vertIdx + i] - anc) * scale + anc;

            for (int c = ep.StartChar + 1; c < ep.StartChar + ep.Length; c++)
            {
                cInfo = tInfo.characterInfo[c];
                vertIdx = cInfo.vertexIndex;

                for (int i = 0; i < 4; i++)
                    vertices[vertIdx + i] = (vertices[vertIdx + i] - anc) * scale + anc;
            }
        }

        static void TransformText(Latex latex, IExpression ep, IExpression baseEp, float scale, Vector2 pos)
        {
            float anchor = (baseEp.Baseline - baseEp.BottomRight.y) / (baseEp.TopLeft.y - baseEp.BottomRight.y);
            TransformText(latex, ep, baseEp, scale, pos, anchor);
        }

        static void TransformText(Latex latex, IExpression ep, IExpression baseEp, float scale, Vector2 pos, float anchorCoef)
        {
            var tInfo = latex.tInfo;
            var cInfo = tInfo.characterInfo[ep.StartChar];
            var vertIdx = cInfo.vertexIndex;
            var vertices = tInfo.meshInfo[cInfo.materialReferenceIndex].vertices;
            var pos3 = new Vector3(pos.x, pos.y);

            Vector3 oldAnchor = baseEp.BottomLeft;
            oldAnchor.y += (baseEp.TopLeft.y - baseEp.BottomRight.y) * anchorCoef;
            for (int i = 0; i < 4; i++)
                vertices[vertIdx + i] = (vertices[vertIdx + i] - oldAnchor) * scale + pos3;

            for (int c = ep.StartChar + 1; c < ep.StartChar + ep.Length; c++)
            {
                cInfo = tInfo.characterInfo[c];
                vertIdx = cInfo.vertexIndex;

                for (int i = 0; i < 4; i++)
                    vertices[vertIdx + i] = (vertices[vertIdx + i] - oldAnchor) * scale + pos3;
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