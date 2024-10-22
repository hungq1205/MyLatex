using UnityEngine;
using TMPro;
using System.Text;

namespace Latex
{
    public class ExpressionBase : IExpression
    {
        public string Content { get; protected set; }
        public int StartChar { get; protected set; }
        public int Length { get; protected set; }

        public ExpressionBase(string content)
        {
            Content = content;
        }

        public virtual void Build(StringBuilder sb)
        {
            Debug.LogError("Building unknown expression");
        }

        public virtual void Render(TMP_TextInfo tInfo) 
        {
            if (StartChar < 0 || Length < 0)
                throw new System.Exception("Expression has not been built");
        }
    }
}