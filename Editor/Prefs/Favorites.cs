namespace TF.SceneBrowser.Editor
{
    using UnityEngine;

    internal static class Favorites
    {
        private const string PREFS_FAVORITE_SCENE = "SceneBrowser_FavoriteScene";
        private const char SEPARATOR = ';';
        private static readonly EditorPrefsList _favoritesScenesPath = new(PREFS_FAVORITE_SCENE, SEPARATOR);

        public static void FavoriteScene(SceneData sceneAsset)
        {
            if (IsSceneFavorite(sceneAsset) == true)
            {
                Debug.LogWarningFormat("Scene {0} is already favorite. Can't favorite it again.", sceneAsset.Name);
                return;
            }

            _favoritesScenesPath.Add(sceneAsset.Path);
        }

        public static void UnfavoriteScene(SceneData sceneAsset) => _favoritesScenesPath.Remove(sceneAsset.Path);
        public static bool IsSceneFavorite(SceneData sceneAsset) => _favoritesScenesPath.Contains(sceneAsset.Path);

        public static void ToggleFavorite(SceneData sceneAsset)
        {
            if (IsSceneFavorite(sceneAsset) == true)
            {
                UnfavoriteScene(sceneAsset);
            }
            else
            {
                FavoriteScene(sceneAsset);
            }
        }
    }
}
