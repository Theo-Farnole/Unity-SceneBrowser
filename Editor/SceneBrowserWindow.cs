namespace TF.SceneBrowser.Editor
{
    using System;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;

    internal class SceneBrowserWindow : EditorWindow
    {

        [SerializeField]
        private SceneData[] _projectScenes = null;

        [SerializeField]
        private Vector2 _scrollPosition = Vector2.zero;

        [SerializeField] private string _searchString = "";

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
            DrawToolbar();
            DrawContent();
        }

        private void DrawContent()
        {
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
            {
                if (_searchString != "")
                {
                    var results = SearchScenes(_searchString);

                    GUILayout.Label(string.Format("Results ({0})", results.Length), EditorStyles.boldLabel);
                    DrawScenesList(results);
                }
                else
                {
                    SceneData loadedScene = GetLoadedScene();

                    if (loadedScene != null)
                    {
                        GUILayout.Label("Loaded Scenes", EditorStyles.boldLabel);
                        SceneDataGUI.SceneControls(loadedScene);
                    }

                    GUIHelper.DrawSeparator();


                    DrawFavoritesScenes();

                    GUILayout.Label("All Scenes", EditorStyles.boldLabel);
                    DrawScenesList(GetNotFavoritesScenes());
                }
            }
            GUILayout.EndScrollView();
        }

        private void DrawToolbar()
        {
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            {
                _searchString = GUILayout.TextField(_searchString, GUI.skin.FindStyle("ToolbarSeachTextField"), GUILayout.Width(250));

                if (_searchString != "")
                {
                    if (GUILayout.Button("", GUI.skin.FindStyle("ToolbarSeachCancelButton")))
                    {
                        _searchString = "";
                    }
                }

                GUILayout.FlexibleSpace();

                if (GUILayout.Button(Icons.Refresh, EditorStyles.toolbarButton))
                {
                    SetScenesAssets();
                }
            }
            GUILayout.EndHorizontal();
        }

        private SceneData[] SearchScenes(string searchTerm)
        {
            return _projectScenes
                .Where(x => CaseInsensitiveContains(x.Name, searchTerm))
                .ToArray();

            static bool CaseInsensitiveContains(string text, string value)
            {
                return text.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0;
            }
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
            .FirstOrDefault();
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
                SceneDataGUI.SceneControls(scene);
            }
        }


        private void SetScenesAssets()
        {
            _projectScenes = Utils.GetAllSceneDatas();
        }
    }
}