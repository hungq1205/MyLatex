using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
using System.Text;

namespace Latex
{
    public class Latex : MonoBehaviour
    {
        public TextMeshProUGUI tmp;
        public float characterSpacing = 0.5f; 
        public float param = 5f;
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
            IExpression[] eps = new IExpression[3];
            for (int i = 0; i < eps.Length; i++)
                eps[i] = new TextExpression(content.Split(' ')[i]);

            //IExpression sqrt = new SquareRoot(eps[0]);
            IExpression frac = new Fraction(new SquareRoot(eps[1]), new SquareRoot(eps[2]));

            StringBuilder sb = new();
            //sqrt.Build(sb);
            frac.Build(sb);

            tmp.text = sb.ToString();
            sb.Clear();
            tmp.ForceMeshUpdate();

            //sqrt.Render(this);
            frac.Render(this);

            tmp.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
        }
    }
}