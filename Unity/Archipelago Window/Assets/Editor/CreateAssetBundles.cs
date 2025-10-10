using System.Diagnostics;
using UnityEditor;

public class CreateAssetBundles
{
    [MenuItem("Bundler/Build Asset Bundles")]
    static void BuildAllAssetBundles()
    {
        BuildPipeline.BuildAssetBundles("Assets/Dist", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
    }
}