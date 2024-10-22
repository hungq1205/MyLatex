using Latex;
using TMPro;

namespace Latex
{
    public interface ITransform
    {
        void Perform(TMP_TextInfo tInfo, int startChar, int length);
    }
}