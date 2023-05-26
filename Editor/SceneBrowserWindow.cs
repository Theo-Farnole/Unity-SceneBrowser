namespace TF.SceneBrowser.Editor
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEditor;
    using UnityEditor.SceneManagement;
    using UnityEngine;

    internal class SceneBrowserWindow : EditorWindow
    {

        [SerializeField]
        private SceneData[] _projectScenes = null;

        [SerializeField]
        private Vector2 _scrollPosition = Vector2.zero;

        [MenuItem("Tools/SceneBrowser &b")]
        public static void OpenWindow()
        {
            GetWindow<SceneBrowserWindow>("Scenes Browser")
                .Show();
        }

        private void OnEnable()
        {
            if (_projectScenes == null || _projectScenes.Length == 0)
            {
                SetScenesAssets();
            }
        }

        protected void OnGUI()
        {
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            {
                GUILayout.Label("Scenes Browser", EditorStyles.boldLabel);

                GUILayout.FlexibleSpace();

                if (GUILayout.Button(Icons.Refresh, EditorStyles.toolbarButton))
                {
                    SetScenesAssets();
                }
            }
            GUILayout.EndHorizontal();


            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
            {
                GUILayout.Label("Loaded Scenes", EditorStyles.boldLabel);
                SceneDataGUI.DrawLayout(GetLoadedScene());

                GUIHelper.DrawSeparator();


                DrawFavoritesScenes();

                GUILayout.Label("All Scenes", EditorStyles.boldLabel);
                DrawScenesList(GetNotFavoritesScenes());
            }
            GUILayout.EndScrollView();
        }


        private void DrawFavoritesScenes()
        {
            SceneData[] favoritesScenes = GetOnlyFavoritesScenes();

            if (favoritesScenes.Length != 0)
            {
                GUILayout.Label("Pinned Scenes", EditorStyles.boldLabel);
                DrawScenesList(favoritesScenes);

                GUIHelper.DrawSeparator();
            }
        }

        private SceneData GetLoadedScene()
        {
            return _projectScenes
            .Where(x => x.Path == UnityEngine.SceneManagement.SceneManager.GetActiveScene().path)
            .First();
        }

        private SceneData[] GetNotFavoritesScenes()
        {
            return _projectScenes.Where(x => Favorites.IsSceneFavorite(x) == false).ToArray();
        }

        private SceneData[] GetOnlyFavoritesScenes()
        {
            return _projectScenes.Where(x => Favorites.IsSceneFavorite(x) == true).ToArray();
        }

        private void DrawScenesList(SceneData[] scenesToDraw)
        {
            foreach (SceneData scene in scenesToDraw)
            {
                SceneDataGUI.DrawLayout(scene);
            }
        }


        private void SetScenesAssets()
        {
            _projectScenes = Utils.GetAllSceneDatas();
        }
    }
}