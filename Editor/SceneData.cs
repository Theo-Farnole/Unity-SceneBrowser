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

        public void DrawLayout()
        {


        }
    }

    public static class SceneDataGUI
    {
        private const int WIDTH_PX_BUTTONS = 100;
        private const int SPACING_PX_BUTTONS = 3;


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
            Texture buttonTexture = Favorites.IsSceneFavorite(sceneAsset) ? SceneBrowserResources.GetFullStarTexture() : SceneBrowserResources.GetEmptyStarTexture();


            bool clickOnFavoriteButton = GUILayout.Button(new GUIContent(buttonTexture), GUILayout.Height(EditorGUIUtility.singleLineHeight), GUILayout.Width(EditorGUIUtility.singleLineHeight + 15));

            if (clickOnFavoriteButton == true)
            {
                Favorites.ToggleFavorite(sceneAsset);
            }
        }

        private static void DrawOpenButtons(SceneData sceneAsset)
        {
            if (sceneAsset.IsLoaded == true)
            {
                bool shouldClose = GUIHelper.DrawColoredButton("Close", Color.red, GUILayout.Width(WIDTH_PX_BUTTONS * 2 + SPACING_PX_BUTTONS), GUILayout.ExpandWidth(false));

                if (shouldClose)
                {
                    sceneAsset.CloseScene();
                }
            }
            else
            {
                bool shouldOpenScene = GUILayout.Button("Open", GUILayout.Width(WIDTH_PX_BUTTONS), GUILayout.ExpandWidth(false));

                if (shouldOpenScene)
                {
                    sceneAsset.OpenScene(OpenSceneMode.Single);
                }
                else
                {


                    bool additiveOpen = GUILayout.Button("Additive Open", GUILayout.Width(WIDTH_PX_BUTTONS), GUILayout.ExpandWidth(false));

                    if (additiveOpen)
                    {
                        sceneAsset.OpenScene(OpenSceneMode.Additive);
                    }
                }
            }
        }
    }
}