namespace TF.SceneBrowser.Editor
{
    using System.Collections.Generic;
    using UnityEditor;

    internal class EditorPrefsList
    {
        public readonly string key;
        public readonly char separator;

        private readonly List<string> _data;

        public EditorPrefsList(string key, char separator)
        {
            this.key = key;
            this.separator = separator;

            string fromSaveString = EditorPrefs.GetString(key);

            string[] scenesPath = fromSaveString.Split(separator);
            _data = new List<string>(scenesPath);
        }

        public void Add(string value)
        {
            _data.Add(value);
            SaveChanges();
        }

        public void Remove(string value)
        {
            _data.Remove(value);
            SaveChanges();
        }

        public bool Contains(string value) => _data.Contains(value);

        private void SaveChanges()
        {
            string joinedFavoritesScenesPaths = string.Join(separator.ToString(), _data);
            EditorPrefs.SetString(key, joinedFavoritesScenesPaths);
        }
    }
}
