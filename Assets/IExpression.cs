using System.Text;
using TMPro;
using UnityEngine;

namespace Latex
{
    public interface IExpression
    {
        IExpression[] Content { get; }
        Vector2 TopLeft { get; }
        Vector2 BottomRight { get; }
        Vector2 TopRight => new Vector2(BottomRight.x, TopLeft.y);
        Vector2 BottomLeft => new Vector2(TopLeft.x, BottomRight.y);

        void Build(StringBuilder sb);
        void Render(Latex latex, IExpression preceeding = null);
        void Transform(Latex latex, float scale, IExpression preceeding = null);
        void Transform(Latex latex, float scale, Vector2 pos, float anchorCoef = 0f);
        void UpdateBound(Latex latex);
    }
}