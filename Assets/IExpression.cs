using System.Text;
using TMPro;

namespace Latex
{
    public interface IExpression
    {
        string Content { get; }

        /// <summary>
        /// Build expression text (add notions, symbols or shapes)
        /// </summary>
        /// <returns>Number of added character index</returns>
        void Build(StringBuilder sb);
        void Render(TMP_TextInfo tInfo);
    }
}