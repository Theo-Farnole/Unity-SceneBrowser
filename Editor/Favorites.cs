namespace TF.SceneBrowser.Editor
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using UnityEditor;
	using UnityEngine;

	internal static class Favorites
	{
		private const string PREFS_FAVORITE_SCENE = "SceneBrowser_FavoriteScene";
		private const char SEPARATOR = ';';
		private static List<string> _favoritesScenesPath = null;

		private static List<string> FavoritesScenesPath
		{
			get
			{
				if (_favoritesScenesPath == null)
				{

					if (EditorPrefs.HasKey(PREFS_FAVORITE_SCENE) == false)
					{
						Debug.LogWarning("Cannot retrives favorites preferences. They might been resetted");
					}

					string fromSaveString = EditorPrefs.GetString(PREFS_FAVORITE_SCENE);

					string[] scenesPath = fromSaveString.Split(SEPARATOR);
					_favoritesScenesPath = new List<string>(scenesPath);
				}

				return _favoritesScenesPath;
			}

			set
			{
				_favoritesScenesPath = value;

				string joinedFavoritesScenesPaths = string.Join(SEPARATOR.ToString(), _favoritesScenesPath);
				EditorPrefs.SetString(PREFS_FAVORITE_SCENE, joinedFavoritesScenesPaths);
			}
		}

		public static void FavoriteScene(SceneData sceneAsset)
		{
			if (IsSceneFavorite(sceneAsset) == true)
			{
				Debug.LogWarningFormat("Scene {0} is already favorite. Can't favorite it again.", sceneAsset.Name);
				return;
			}

			FavoritesScenesPath.Add(sceneAsset.Path);
		}

		public static void UnfavoriteScene(SceneData sceneAsset)
		{
			FavoritesScenesPath.Remove(sceneAsset.Path);
		}

		public static bool IsSceneFavorite(SceneData sceneAsset)
		{
			return FavoritesScenesPath.Contains(sceneAsset.Path);
		}

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
