﻿namespace TF.SceneBrowser.Editor
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
            var window = GetWindow<SceneBrowserWindow>();
            window.Show();
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
            GUILayout.Label("Scenes Browser", EditorStyles.boldLabel);

            GUIHelper.DrawSeparator();

            if (GUILayout.Button("Refresh"))
            {
                SetScenesAssets();
            }

            GUIHelper.DrawSeparator();

            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
            {
                DrawFavoritesScenes();

                DrawScenesContent(GetNotFavoritesScenes());
            }
            GUILayout.EndScrollView();
        }


        private void DrawFavoritesScenes()
        {
            SceneData[] favoritesScenes = GetOnlyFavoritesScenes();

            if (favoritesScenes.Length != 0)
            {
                DrawScenesContent(favoritesScenes);

                GUIHelper.DrawSeparator();
            }
        }

        private SceneData[] GetNotFavoritesScenes()
        {
            return _projectScenes.Where(x => Favorites.IsSceneFavorite(x) == false).ToArray();
        }

        private SceneData[] GetOnlyFavoritesScenes()
        {
            return _projectScenes.Where(x => Favorites.IsSceneFavorite(x) == true).ToArray();
        }

        private void DrawScenesContent(SceneData[] scenesToDraw)
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