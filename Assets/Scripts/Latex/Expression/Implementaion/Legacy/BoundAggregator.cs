using System.Text;
using UnityEngine;

namespace Latex
{
    public class BoundAggregator : IExpression
    {
        public bool takeMax; 

        public BoundAggregator(bool takeMax = true, params IExpression[] targets) : base()
        {
            this.takeMax = takeMax;
            Content = targets;
        }

        public IExpression[] Content { get; private set; }
        public int StartChar => -1;
        public int Length => 0;
        public float Scale => 0;
        public float SpacingLeft { get; set; }
        public float SpacingRight { get; set; }

        public float Baseline => 0;

        private Vector2 topLeft;
        public Vector2 TopLeft => topLeft;

        public Vector2 bottomRight;
        public Vector2 BottomRight => bottomRight;

        public void Build(StringBuilder sb) { }

        public void Render(Latex latex, IExpression preceeding = null)
            => UpdateBound(latex);

        public void Transform(Latex latex, float scale, IExpression preceeding = null, IExpression baseEp = null)
            => UpdateBound(latex);

        public void Transform(Latex latex, float scale, Vector2 pos, IExpression baseEp = null)
            => UpdateBound(latex);

        public void Transform(Latex latex, float scale, Vector2 pos, float anchorCoef, IExpression baseEp = null)
            => UpdateBound(latex);

        public void UpdateBound(Latex latex)
        {
            if (takeMax)
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
            else
            {
                topLeft = new Vector2(float.NegativeInfinity, float.PositiveInfinity);
                bottomRight = new Vector2(float.PositiveInfinity, float.NegativeInfinity);

                foreach (IExpression ep in Content)
                {
                    if (ep.TopLeft.x > topLeft.x) topLeft.x = ep.TopLeft.x;
                    if (ep.TopLeft.y < topLeft.y) topLeft.y = ep.TopLeft.y;
                    if (ep.BottomRight.x < bottomRight.x) bottomRight.x = ep.BottomRight.x;
                    if (ep.BottomRight.y > bottomRight.y) bottomRight.y = ep.BottomRight.y;
                }
            }
        }
    }
}
