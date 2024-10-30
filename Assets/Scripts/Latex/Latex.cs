using UnityEngine;
using TMPro;
using System.Text;
using System.Collections.Generic;

namespace Latex
{
    public class Latex : MonoBehaviour
    {
        public TextMeshProUGUI tmp;
        //[Range(0, 1f)] public float param = 1f;
        [Range(-100f, 100f)] public float param = 5f;
        [TextArea(5, 1000)] public string content;

        [HideInInspector] public TMP_TextInfo tInfo;

        void Start()
        {
            tInfo = tmp.textInfo;
            Refresh();
        }

        public void Refresh()
        {
            if (string.IsNullOrEmpty(content))
            {
                tmp.text = "";
                return;
            }

            var root = BuildExpressionTree();
            var ep = root.Build();

            StringBuilder sb = new();
            ep.Build(sb);

            tmp.text = sb.ToString();
            sb.Clear();
            tmp.ForceMeshUpdate();

            ep.Render(this);

            var rTransform = (RectTransform)transform;
            ExpressionBase.HorizontalAlign(this, ep, rTransform.rect.xMin, rTransform.rect.xMax);

            tmp.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
        }

        ExpressionNode BuildExpressionTree()
        {
            Stack<ExpressionNode> traces = new();
            traces.Push(new ExpressionNode("\\group"));

            StringBuilder epSb = new();
            string text = content;

            bool escape = false, notified = false;
            for (int i = 0; i < text.Length; i++)
            {
                if (escape)
                {
                    if (text[i] == Utility.EscapeChar || 
                        text[i] == '{' || text[i] == '}' ||
                        Utility.Notifiers.Contains(text[i]))
                    {
                        epSb.Append(text[i]);
                        notified = false;
                    }
                    else
                    {
                        if (epSb.Length > 0)
                        {
                            traces.Peek().childs.AddLast(new ExpressionNode(epSb.ToString(), true));
                            epSb.Clear();
                        }
                        epSb.Append(Utility.EscapeChar);
                        epSb.Append(text[i]);
                    }
                    escape = false;
                    continue;
                }

                if (text[i] == Utility.EscapeChar)
                {
                    escape = true;
                    if (Utility.Notifiers.Contains(Utility.EscapeChar))
                        notified = true;
                    continue;
                }

                if (Utility.Notifiers.Contains(text[i]))
                {
                    notified = true;
                    if (epSb.Length > 0)
                    {
                        traces.Peek().childs.AddLast(new ExpressionNode(epSb.ToString(), true));
                        epSb.Clear();
                    }
                    epSb.Append(text[i]);
                    continue;  
                }

                if (text[i] == '{')
                {
                    ExpressionNode node;
                    if (notified)
                    {
                        node = new ExpressionNode(epSb.ToString());
                        traces.Peek().childs.AddLast(node);
                        traces.Push(node);
                        notified = false;
                    }
                    node = new ExpressionNode("\\group");
                    traces.Peek().childs.AddLast(node);
                    traces.Push(node);
                    epSb.Clear();
                }
                else if (text[i] == '}') 
                {
                    if (epSb.Length > 0)
                        traces.Peek().childs.AddLast(new ExpressionNode(epSb.ToString(), true));
                    if (i + 1 >= text.Length || text[i + 1] != '{')
                        traces.Pop();
                    traces.Pop();

                    epSb.Clear();
                }
                else if (text[i] == ' ')
                {
                    if (notified)
                    {
                        traces.Peek().childs.AddLast(new ExpressionNode(epSb.ToString()));
                        epSb.Clear();
                    }
                    else
                        epSb.Append('\u00A0');
                }
                else
                    epSb.Append(text[i]);
            }
            if (epSb.Length > 0)
                traces.Peek().childs.AddLast(new ExpressionNode(epSb.ToString(), true));
            epSb.Clear();

            ExpressionNode cur;
            do
            {
                cur = traces.Pop();
            } while (traces.Count != 0);

            return cur;
        }

        class ExpressionNode
        {
            public string context;
            public LinkedList<ExpressionNode> childs;

            public ExpressionNode(string context, bool textOnly=false)
            {
                this.context = context;
                if (!textOnly)
                    childs = new();
            }

            public IExpression Build()
            {
                if (childs != null)
                {
                    if (childs.Count == 1 && childs.First.Value.context == "\\group")
                        return Utility.InstantiateExpression(context, new[] { childs.First.Value.Build() });

                    List<IExpression> childBuilds = new();
                    foreach (var c in childs)
                        childBuilds.Add(c.Build());
                    return Utility.InstantiateExpression(context, childBuilds.ToArray());
                }

                return context.Length == 1 ? new CharExpression(context[0]) : new TextExpression(context);
            }
        }
    }
}