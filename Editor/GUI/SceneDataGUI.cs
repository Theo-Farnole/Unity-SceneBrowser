namespace TF.SceneBrowser.Editor
{
    using UnityEditor;
    using UnityEditor.SceneManagement;
    using UnityEngine;

    public static class SceneDataGUI
    {
        private const int WIDTH_PX_BUTTONS = 70;

        public static void SceneControls(SceneData sceneAsset)
        {
            GUILayout.BeginHorizontal();
            {
                FavoriteButton(sceneAsset);
                if (GUILayout.Button(sceneAsset.Name, "Label"))
                {
                    sceneAsset.OpenScene(OpenSceneMode.Single);
                }

                SideButtons(sceneAsset);
            }
            GUILayout.EndHorizontal();
        }

        private static void FavoriteButton(SceneData sceneAsset)
        {
            var buttonTexture = Favorites.IsSceneFavorite(sceneAsset) ? Icons.Pinned : Icons.Unpin;


            bool clickOnFavoriteButton = GUILayout.Button(buttonTexture, GUILayout.Height(EditorGUIUtility.singleLineHeight), GUILayout.Width(EditorGUIUtility.singleLineHeight + 15));

            if (clickOnFavoriteButton == true)
            {
                Favorites.ToggleFavorite(sceneAsset);
            }
        }

        private static void SideButtons(SceneData sceneAsset)
        {
            if (GUILayout.Button("Select", GUILayout.Width(WIDTH_PX_BUTTONS), GUILayout.ExpandWidth(false)))
            {
                HighlightInProjectWindow(sceneAsset);
            }

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
                int clickedButton = GUILayout.Toolbar(-1, new string[] { "Open", "Add" }, GUILayout.Width(WIDTH_PX_BUTTONS * 2));

                switch (clickedButton)
                {
                    case 0:
                        sceneAsset.OpenScene(OpenSceneMode.Single);
                        break;

                    case 1:
                        sceneAsset.OpenScene(OpenSceneMode.Additive);
                        break;
                }
            }
        }

        private static void HighlightInProjectWindow(SceneData sceneAsset)
        {
            Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(sceneAsset.Path);
        }
    }
}