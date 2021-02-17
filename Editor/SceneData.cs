namespace TF.SceneBrowser.Editor
{
	using UnityEditor.SceneManagement;
	using UnityEngine.SceneManagement;

	public class SceneData
	{
		#region Fields
		private readonly string _name = null;
		private readonly string _path = null;
		#endregion Fields

		public string Name => _name;
		public string Path => _path;
		public bool IsLoaded => Scene.IsValid() && Scene.isLoaded;
		public Scene Scene => SceneManager.GetSceneByPath(_path);

		public SceneData(string sceneAssetPath)
		{
			_name = SceneBrowserUtils.GetFileNameWithoutExtension(sceneAssetPath);
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
	}
}
