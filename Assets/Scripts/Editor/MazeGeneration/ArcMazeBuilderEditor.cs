using UnityEngine;
using UnityEditor;
using Unity.EditorCoroutines.Editor;
using UnityEngine.UIElements;
using System.Linq;
using Zenject;

namespace KarenKrill.MazeGeneration
{
    [CustomEditor(typeof(ArcMazeBuilder))]
    public class ArcMazeBuilderEditor : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var type = typeof(ArcMazeBuilder);
            var loggerField = type.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .Where(field => field.FieldType == typeof(ILogger) &&
                    field.CustomAttributes.Select(attr => attr.AttributeType).Contains(typeof(InjectAttribute)))
                .FirstOrDefault();
            loggerField?.SetValue((ArcMazeBuilder)target, new Logger(new Logging.DebugLogHandler()));
            return base.CreateInspectorGUI();
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var arcMazeBuilder = (ArcMazeBuilder)target;
            if (GUILayout.Button("Rebuild"))
            {
                EditorCoroutineUtility.StartCoroutine(arcMazeBuilder.RebuildCoroutine(), this);
            }
            if (GUILayout.Button("Destroy"))
            {
                EditorCoroutineUtility.StartCoroutine(arcMazeBuilder.DestroyCoroutine(), this);
            }
        }
    }
}
