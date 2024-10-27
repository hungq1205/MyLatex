using UnityEditor;
using UnityEngine;

namespace Latex
{
    [CustomEditor(typeof(Latex))]
    class LatexEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var latex = (Latex)target;

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Compile", GUILayout.Height(50), GUILayout.Width(200)))
            {
                latex.tInfo = latex.tmp.textInfo;
                latex.Refresh();
                EditorUtility.SetDirty(latex.tmp);
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
    }
}
