#if UNITY_EDITOR
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using Unity.EditorCoroutines.Editor;

namespace KarenKrill.TheLabyrinth.MazeGeneration
{
    using Logging;

    [CustomEditor(typeof(ArcMazeBuilder))]
    public class ArcMazeBuilderEditor : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var type = typeof(ArcMazeBuilder);
            var loggerField = type.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .Where(field => field.FieldType == typeof(ILogger))
                .FirstOrDefault();
            loggerField?.SetValue((ArcMazeBuilder)target, new Logger(new DebugLogHandler()));
            return base.CreateInspectorGUI();
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var arcMazeBuilder = (ArcMazeBuilder)target;
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Rebuild"))
            {
                EditorCoroutineUtility.StartCoroutine(arcMazeBuilder.RebuildCoroutine(), this);
            }
            if (GUILayout.Button("Destroy"))
            {
                EditorCoroutineUtility.StartCoroutine(arcMazeBuilder.DestroyCoroutine(), this);
            }
            GUILayout.EndHorizontal();
        }
    }
}
#endif