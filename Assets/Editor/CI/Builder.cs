using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CI
{
    public class Builder
    {
        public static void Build()
        {
            var name = Application.productName;
            BuildPipeline.BuildPlayer(ScenePaths, $"Build/StandaloneWindows/{name}.exe", BuildTarget.StandaloneWindows, BuildOptions.CompressWithLz4);
        }

        static string[] ScenePaths => EditorBuildSettings.scenes.Select(scene => scene.path).ToArray();
    }
}
