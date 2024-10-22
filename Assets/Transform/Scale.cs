using TMPro;
using UnityEngine;

namespace Latex 
{
    public enum Anchor
    {
        Top,
        Middle,
        Bottom,
    }

    public class Scale : ITransform
    {
        public float factor;
        public Anchor anchor;

        public Scale(float factor, Anchor anchor = Anchor.Bottom)
        {
            this.factor = factor;
            this.anchor = anchor;
        }

        public void Perform(TMP_TextInfo tInfo, int startChar, int length)
        {
            TMP_CharacterInfo cInfo;
            Vector3 anc, offset = Vector3.zero;
            Vector3[] vertices;
            int vertIdx;
            float ancCoef = AnchorCoef(anchor);

            for (int c = startChar; c < startChar + length; c++)
            {
                cInfo = tInfo.characterInfo[c];
                vertIdx = cInfo.vertexIndex;

                Vector3 newOffset = new((1 - factor) * (cInfo.bottomLeft - cInfo.bottomRight).x, 0);
                Debug.Log(newOffset);
                Debug.Log(cInfo.bottomLeft - cInfo.bottomRight);

                vertices = tInfo.meshInfo[cInfo.materialReferenceIndex].vertices;
                anc = cInfo.bottomLeft + (cInfo.topLeft - cInfo.bottomLeft) * ancCoef;

                for (int i = 0; i < 4; i++)
                    vertices[vertIdx + i] = (vertices[vertIdx + i] - anc) * factor + anc + offset;

                offset += newOffset;
            }

            for (int c = startChar + length; c < tInfo.characterCount; c++)
            {
                cInfo = tInfo.characterInfo[c];
                vertices = tInfo.meshInfo[cInfo.materialReferenceIndex].vertices;
                vertIdx = cInfo.vertexIndex;
                for (int i = 0; i < 4; i++)
                    vertices[vertIdx + i] += offset;
            }
        }

        static float AnchorCoef(Anchor anchor)
        {
            switch (anchor)
            {
                case Anchor.Bottom: return 0;
                case Anchor.Middle: return 0.5f;
                case Anchor.Top: return 1f;
                default:
                    throw new System.Exception("Anchor not found");
            }
        }
    }
}
