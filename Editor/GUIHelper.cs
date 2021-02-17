namespace TF.SceneBrowser.Editor
{
	using UnityEditor;
	using UnityEngine;

	public class GUIHelper
	{
		public static void DrawSeparator()
		{
			EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
		}

		public static bool DrawColoredButton(string name, Color color, params GUILayoutOption[] layoutOptions)
		{
			GUI.backgroundColor = color;

			bool buttonClicked = GUILayout.Button(name, layoutOptions);

			GUI.backgroundColor = Color.white;

			return buttonClicked;
		}
	}
}
