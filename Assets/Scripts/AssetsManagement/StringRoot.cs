namespace UnityEngine.UI
{
    [CreateAssetMenu(menuName = "Assets/String Root")]
    public class StringRoot : ScriptableObject
    {
        public static StringRoot Instance => AssetRoot.Instance.StringRoot;
        
        [TextArea]
        public string GameOverCountDownText;
        [TextArea]
        public string GameOverText;
        [TextArea]
        public string BadSignalWarning;
    }
}