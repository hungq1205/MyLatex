using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
using System.Text;

namespace Latex
{
    public class Latex : MonoBehaviour
    {
        public TextMeshProUGUI tmp;
        [TextArea(5, 1000)] public string content;

        void Start()
        {
            Refresh();
        }

        float count = 0;
        void Update()
        {
            count -= Time.deltaTime;
            if (count <= 0)
            {
                count = 1f;
                //Refresh();
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

            ep.Render(tmp.textInfo);

            tmp.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
        }
    }
}