namespace TF.SceneBrowser.Editor
{
	using UnityEditor;
	using UnityEditor.SceneManagement;
	using UnityEngine;

	public class SceneBrowserWindow : EditorWindow
	{
		private const int WIDTH_PX_BUTTONS = 100;
		private const int SPACING_PX_BUTTONS = 3;
		private SceneData[] _projectScenes = null;

		[SerializeField]
		private Vector2 _scrollPosition = Vector2.zero;

		[MenuItem("Tools/Open SceneBrowser &b")]
		public static void OpenWindow()
		{
			var window = GetWindow<SceneBrowserWindow>();
			window.Show();
		}

		private void OnEnable()
		{
			if (_projectScenes == null)
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
				foreach (SceneData scene in _projectScenes)
				{
					DrawScene(scene);
				}
			}
			GUILayout.EndScrollView();
		}

		private void DrawScene(SceneData sceneAsset)
		{
			GUILayout.BeginHorizontal();
			{
				GUILayout.Label(sceneAsset.Name);
				DrawButtons(sceneAsset);
			}
			GUILayout.EndHorizontal();
		}

		private static void DrawButtons(SceneData sceneAsset)
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

		private void SetScenesAssets()
		{
			_projectScenes = SceneBrowserUtils.GetAllSceneDatas();
		}
	}
}