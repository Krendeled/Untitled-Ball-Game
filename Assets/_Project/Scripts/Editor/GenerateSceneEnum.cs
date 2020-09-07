using System.IO;
using UnityEditor;
using UnityEngine.SceneManagement;
using UntitledBallGame.SceneManagement;

namespace UntitledBallGame.Editor
{
    public static class GenerateSceneEnum
    {
        private static readonly string scenePath = "Assets/_Project/Scenes/";
        private static readonly string enumName = "GameScene";
        private static readonly string enumPath = $"Assets/_Project/Scripts/SceneManagement/{enumName}.cs";

        [MenuItem("Tools/Generate Scene Enum")]
        public static void Go()
        {
            string[] enumEntries = new string[SceneManager.sceneCountInBuildSettings];

            for (int i = 0; i < enumEntries.Length; i++)
            {
                string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                enumEntries[i] = SceneHelper.GetNameFromPath(scenePath);
            }

            using (StreamWriter streamWriter = new StreamWriter(enumPath))
            {
                streamWriter.WriteLine("public enum " + enumName);
                streamWriter.WriteLine("{");
                for (int i = 0; i < enumEntries.Length; i++)
                {
                    streamWriter.WriteLine("\t" + enumEntries[i] + ",");
                }

                streamWriter.WriteLine("}");
            }

            AssetDatabase.Refresh();
        }
    }
}