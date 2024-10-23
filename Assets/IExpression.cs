using System.Text;
using TMPro;
using UnityEngine;

namespace Latex
{
    public interface IExpression
    {
        IExpression[] Content { get; }
        bool Isolated { get; }
        Vector2 TopLeft { get; }
        Vector2 BottomRight { get; }

        void Build(StringBuilder sb);
        void Render(Latex latex);
        void Transform(Latex latex, float scale);
        void Transform(Latex latex, float scale, Vector2 offset);
        void UpdateBound(Latex latex);
    }
}