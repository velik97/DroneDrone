using UnityEditor;
using UnityEngine.UI;

namespace Editor
{
    [CustomEditor(typeof(AssetRoot))]
    public class AssetRootEditor : UnityEditor.Editor
    {
        private AssetRoot m_Target;
        
        private void OnEnable()
        {
            m_Target = target as AssetRoot;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            string currentPath = AssetDatabase.GetAssetPath(m_Target);

            if (string.CompareOrdinal(currentPath, AssetRoot.ASSET_ROOT_PATH) != 0)
            {
                EditorGUILayout.HelpBox("File location or name is not correct\n" +
                                        $"Current: {currentPath}\n" +
                                        $"Correct: {AssetRoot.ASSET_ROOT_PATH}",
                                        MessageType.Error, true);
            }
        }
    }
}