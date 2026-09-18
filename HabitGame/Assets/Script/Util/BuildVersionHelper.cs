#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

// NOTE
// PlayerSettings에서 직접 관리하는 게 좋지 않다.
// 자동으로 앱의 패치 버전을 관리하도록 만든다.
public static class BuildVersionHelper
{
    #region 1. Fields

    private const int VERSION_PART_COUNT = 3;
    private const int MAX_PATCH_VERSION = 99;

    #endregion

    #region 2. MenuItems

    [MenuItem("Build/Log Current Version", false, 0)]
    public static void LogCurrentVersion()
    {
        Debug.Log($"Current Patch Version: {GetCurrentBundleVersion()}");
    }

    [MenuItem("Build/Update Major Version", false, 10)]
    public static void UpdateMajorVersion()
    {
    }

    [MenuItem("Build/Update Minor Version", false, 20)]
    public static void UpdateMinorVersion()
    {
        var currentBundleVersion = GetCurrentBundleVersion();
        if (!TryParseBundleVersion(currentBundleVersion, out var majorVersion, out var minorVersion, out _))
        {
            Debug.LogError($"Bundle Version 형식이 올바르지 않습니다: {currentBundleVersion}");
            return;
        }

        var updatedBundleVersion = $"{majorVersion}.{minorVersion + 1}.0";
        PlayerSettings.bundleVersion = updatedBundleVersion;

        Debug.Log($"Bundle Version Updated: {currentBundleVersion} -> {updatedBundleVersion}");
    }
    
    [MenuItem("Build/Update Patch Version", false, 30)]
    public static void UpdatePatchVersion()
    {
        var currentBundleVersion = GetCurrentBundleVersion();
        if (!TryParseBundleVersion(currentBundleVersion, out var majorVersion, out var minorVersion, out var patchVersion))
        {
            Debug.LogError($"Bundle Version 형식이 올바르지 않습니다: {currentBundleVersion}");
            return;
        }

        if (patchVersion >= MAX_PATCH_VERSION)
        {
            Debug.LogError($"Patch Version은 {MAX_PATCH_VERSION}을 초과할 수 없습니다: {currentBundleVersion}");
            return;
        }

        var updatedBundleVersion = $"{majorVersion}.{minorVersion}.{patchVersion + 1}";
        PlayerSettings.bundleVersion = updatedBundleVersion;

        Debug.Log($"Bundle Version Updated: {currentBundleVersion} -> {updatedBundleVersion}");
    }

    [MenuItem("Build/Enable Auto Build Version Update", false, 40)]
    public static void EnableAutoBuildVersionUpdate()
    {
    }

    [MenuItem("Build/Disable Auto Build Version Update", false, 41)]
    public static void DisableAutoBuildVersionUpdate()
    {
    }

    #endregion

    #region 3. Methods

    private static string GetCurrentBundleVersion()
    {
        return PlayerSettings.bundleVersion;
    }

    private static bool TryParseBundleVersion(string bundleVersion, out int majorVersion, out int minorVersion, out int patchVersion)
    {
        majorVersion = 0;
        minorVersion = 0;
        patchVersion = 0;

        var versionParts = bundleVersion.Split('.');
        if (versionParts.Length != VERSION_PART_COUNT)
        {
            return false;
        }

        return int.TryParse(versionParts[0], out majorVersion) && majorVersion >= 0 &&
               int.TryParse(versionParts[1], out minorVersion) && minorVersion >= 0 &&
               int.TryParse(versionParts[2], out patchVersion) && patchVersion >= 0 && patchVersion <= MAX_PATCH_VERSION;
    }

    #endregion
}
#endif
