using System.Text;
using TMPro;
using UnityEngine;

namespace Latex
{
    public interface IExpression
    {
        IExpression[] Content { get; }
        int StartChar { get; }
        int Length { get; }

        float Scale { get; }
        float SpacingLeft { get; }
        float SpacingRight { get; }
        float Baseline { get; }
        Vector2 TopLeft { get; }
        Vector2 BottomRight { get; }
        Vector2 TopRight => new Vector2(BottomRight.x, TopLeft.y);
        Vector2 BottomLeft => new Vector2(TopLeft.x, BottomRight.y);
        Vector2 Position => new Vector2(TopLeft.x, Baseline);

        void Build(StringBuilder sb);
        void Render(Latex latex, IExpression preceeding = null);
        void Transform(Latex latex, float scale, IExpression preceeding = null, IExpression baseEp = null);
        void Transform(Latex latex, float scale, Vector2 pos, IExpression baseEp = null);
        void Transform(Latex latex, float scale, Vector2 pos, float anchorCoef, IExpression baseEp = null);
        void UpdateBound(Latex latex);
    }
}