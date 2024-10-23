using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
using System.Text;

namespace Latex
{
    public class Latex : MonoBehaviour
    {
        public TextMeshProUGUI tmp;
        public float spacing = 0.3f;
        public float scaler = 0.1f;
        public bool update = true;
        [TextArea(5, 1000)] public string content;

        [HideInInspector] public TMP_TextInfo tInfo;

        void Start()
        {
            tInfo = tmp.textInfo;
            Refresh();
        }

        float count = 0;
        void Update()
        {
            count -= Time.deltaTime;
            if (update && count <= 0)
            {
                count = 1f;
                Refresh();
            }
        }

        public void Refresh()
        {
            StringBuilder sb = new();
            IExpression ep = new SquareRoot(content);
            ep.Build(sb);

            tmp.text = sb.ToString();
            sb.Clear();
            tmp.ForceMeshUpdate();

            ep.Render(this);

            tmp.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
        }
    }
}