namespace TF.SceneBrowser.Editor
{
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEditor.SceneManagement;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    [System.Serializable]
    public class SceneData
    {
        #region Fields
        [SerializeField]
        private string _name = null;

        [SerializeField]
        private string _path = null;
        #endregion Fields

        public string Name => _name;
        public string Path => _path;
        public bool IsLoaded => Scene.IsValid() && Scene.isLoaded;
        public Scene Scene => SceneManager.GetSceneByPath(_path);

        public SceneData(string sceneAssetPath)
        {
            _name = Utils.GetFileNameWithoutExtension(sceneAssetPath);
            _path = sceneAssetPath;
        }

        public void CloseScene()
        {
            EditorSceneManager.CloseScene(Scene, true);
        }

        public void OpenScene(OpenSceneMode openingMode)
        {
            EditorSceneManager.OpenScene(Path, openingMode);
        }

        public override bool Equals(object obj)
        {
            return obj is SceneData sceneData && sceneData.Path == _path && sceneData.Name == _name;
        }

        public override int GetHashCode()
        {
            int hashCode = -827305254;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(_name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(_path);
            return hashCode;
        }
    }

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
                int clickedButton = GUILayout.Toolbar(-1, new string[] { "Open", "+" }, GUILayout.Width(WIDTH_PX_BUTTONS * 2));

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
    }
}