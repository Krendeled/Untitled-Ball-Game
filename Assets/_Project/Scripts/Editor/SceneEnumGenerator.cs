using System.IO;
using UnityEditor;
using UnityEngine.SceneManagement;
using UntitledBallGame.SceneManagement;

namespace UntitledBallGame.Editor
{
    static class SceneEnumGenerator
    {
        private const string EnumName = "GameScene";
        private static readonly string EnumPath = $"Assets/_Project/Scripts/SceneManagement/{EnumName}.cs";

        [MenuItem("Tools/Generate Scene Enum")]
        private static void Generate()
        {
            string[] enumEntries = new string[SceneManager.sceneCountInBuildSettings];

            for (int i = 0; i < enumEntries.Length; i++)
            {
                string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                enumEntries[i] = SceneHelper.GetNameFromPath(scenePath);
            }

            using (StreamWriter streamWriter = new StreamWriter(EnumPath))
            {
                streamWriter.WriteLine("public enum " + EnumName);
                streamWriter.WriteLine("{");
                foreach (var entry in enumEntries)
                {
                    streamWriter.WriteLine("\t" + entry + ",");
                }
                streamWriter.WriteLine("}");
            }

            AssetDatabase.Refresh();
        }
    }
}