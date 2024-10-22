using System.Text;
using TMPro;

namespace Latex
{
    public class SquareRoot : ExpressionBase
    {
        const char SqrtChar = '\u221A';

        public SquareRoot(string content) : base(content) { }

        public override void Build(StringBuilder sb)
        {
            StartChar = sb.Length;
            sb.Append(SqrtChar);
            sb.Append(Content);
            Length = sb.Length - StartChar;
        }

        public override void Render(TMP_TextInfo tInfo)
        {
            base.Render(tInfo);

            ITransform scaler = new Scale(0.85f);
            scaler.Perform(tInfo, StartChar + 1, Length - 1);
        }
    }
}
