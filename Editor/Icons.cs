namespace TF.SceneBrowser.Editor
{
    using UnityEditor;
    using UnityEngine;

    internal static class Icons
    {
        public static GUIContent Pinned => EditorGUIUtility.IconContent("Pinned");

        public static GUIContent Unpin => EditorGUIUtility.IconContent("Pin");

        public static GUIContent Refresh => EditorGUIUtility.IconContent("d_Refresh");
    }
}
