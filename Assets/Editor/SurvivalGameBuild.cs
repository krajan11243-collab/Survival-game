using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public static class SurvivalGameBuild
{
    public static void BuildAndroid()
    {
        var scenes = EditorBuildSettings.scenes
            .Where(s => s.enabled && !string.IsNullOrEmpty(s.path))
            .Select(s => s.path)
            .ToArray();

        if (scenes.Length == 0)
            throw new Exception("No enabled scenes were found in Build Settings.");

        const string outputDir = "build/Android";
        Directory.CreateDirectory(outputDir);
        string outputPath = Path.Combine(outputDir, "Survival-game.apk");

        PlayerSettings.applicationIdentifier = "com.krajan11243.survivalgame";
        PlayerSettings.bundleVersion = "1.0.0";
        PlayerSettings.Android.bundleVersionCode = 1;
        EditorUserBuildSettings.buildAppBundle = false;

        var options = new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = outputPath,
            target = BuildTarget.Android,
            options = BuildOptions.None
        };

        var report = BuildPipeline.BuildPlayer(options);

        if (report.summary.result != BuildResult.Succeeded)
            throw new Exception("Android build failed: " + report.summary.result + ". Errors: " + report.SummarizeErrors());

        Debug.Log("APK build succeeded: " + outputPath);
    }
}
