namespace TF.SceneBrowser.Editor
{
    using UnityEditor;
    using UnityEditor.SceneManagement;
    using UnityEngine;

    public static class SceneDataGUI
    {
        private const int WIDTH_PX_BUTTONS = 100;

        public static void DrawLayout(SceneData sceneAsset)
        {
            GUILayout.BeginHorizontal();
            {
                DrawFavoriteButton(sceneAsset);
                GUILayout.Label(sceneAsset.Name);
                DrawOpenButtons(sceneAsset);
            }
            GUILayout.EndHorizontal();
        }

        private static void DrawFavoriteButton(SceneData sceneAsset)
        {
            var buttonTexture = Favorites.IsSceneFavorite(sceneAsset) ? Icons.Pinned : Icons.Unpin;


            bool clickOnFavoriteButton = GUILayout.Button(buttonTexture, GUILayout.Height(EditorGUIUtility.singleLineHeight), GUILayout.Width(EditorGUIUtility.singleLineHeight + 15));

            if (clickOnFavoriteButton == true)
            {
                Favorites.ToggleFavorite(sceneAsset);
            }
        }

        private static void DrawOpenButtons(SceneData sceneAsset)
        {
            if (sceneAsset.IsLoaded == true)
            {
                bool shouldClose = GUIHelper.DrawColoredButton("Close", Color.red, GUILayout.Width(WIDTH_PX_BUTTONS * 2), GUILayout.ExpandWidth(false));

                if (shouldClose)
                {
                    sceneAsset.CloseScene();
                }
            }
            else
            {
                int clickedButton = GUILayout.Toolbar(-1, new string[] { "Open", "+", "Select" }, GUILayout.Width(WIDTH_PX_BUTTONS * 2));

                switch (clickedButton)
                {
                    case 0:
                        sceneAsset.OpenScene(OpenSceneMode.Single);
                        break;

                    case 1:
                        sceneAsset.OpenScene(OpenSceneMode.Additive);
                        break;

                    case 2:
                        Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(sceneAsset.Path);
                        break;
                }
            }
        }
    }
}