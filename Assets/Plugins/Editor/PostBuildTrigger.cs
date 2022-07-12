
#if UNITY_EDITOR
using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
 
public static class FrameworkResolver
{
private const string FRAMEWORK_ORIGIN_PATH = "Assets/Plugins/iOS"; // relative to project folder
private const string FRAMEWORK_TARGET_PATH =  "Frameworks"; // relative to build folder
 
[PostProcessBuild]
public static void OnPostprocessBuild(BuildTarget buildTarget, string path)
{
 
if (buildTarget != BuildTarget.iOS)
    return;
 
string sourcePath = Path.Combine(FRAMEWORK_ORIGIN_PATH, "Instabug-XCFramework/Instabug.xcframework");
string destPath = Path.Combine(FRAMEWORK_TARGET_PATH, "Instabug.framework");
 
string deviceFrameworkPath = "ios-arm64_armv7/Instabug.framework";
string simulatorFrameworkPath = "ios-arm64_i386_x86_64-simulator/Instabug.framework";

iOSSdkVersion target = PlayerSettings.iOS.sdkVersion;
if( target == iOSSdkVersion.DeviceSDK){
sourcePath = Path.Combine(sourcePath, deviceFrameworkPath);
} else if (target == iOSSdkVersion.SimulatorSDK){
sourcePath = Path.Combine(sourcePath, simulatorFrameworkPath);
}
 
CopyAndReplaceDirectory(sourcePath, Path.Combine(path, destPath));
 
string pbxProjectPath = PBXProject.GetPBXProjectPath(path);
PBXProject project = new PBXProject();
 
project.ReadFromFile(pbxProjectPath);
 
#if UNITY_2019_3_OR_NEWER
			string targetGuid = project.GetUnityMainTargetGuid();
#else
			string targetGuid = project.TargetGuidByName(PBXProject.GetUnityTargetName());
#endif

 
string fileGuid = project.AddFile(destPath, destPath, PBXSourceTree.Source);
 
project.AddFileToBuild(targetGuid, fileGuid);
 
project.AddFrameworkToProject(targetGuid, "Instabug.framework", false);
 
project.SetBuildProperty(targetGuid, "FRAMEWORK_SEARCH_PATHS", "$(inherited)");
project.AddBuildProperty(targetGuid, "FRAMEWORK_SEARCH_PATHS", "$(PROJECT_DIR)/Libraries");
 
project.SetBuildProperty(targetGuid, "FRAMEWORK_SEARCH_PATHS", "$(SRCROOT)/Frameworks");
project.AddBuildProperty(targetGuid, "FRAMEWORK_SEARCH_PATHS", "$(inherited)");
 
project.AddBuildProperty(targetGuid, "OTHER_LDFLAGS", "-ObjC");
 
//var phaseGUID = project.GetFrameworksBuildPhaseByTarget(targetGuid);
//project.AddFileToBuildSection(targetGuid, phaseGUID, fileGuid);
 
UnityEditor.iOS.Xcode.Extensions.PBXProjectExtensions.AddFileToEmbedFrameworks(project, targetGuid, fileGuid);
 
project.WriteToFile(pbxProjectPath);

// Read plist
var plistPath = Path.Combine(path, "Info.plist");
var plist = new PlistDocument();
plist.ReadFromFile(plistPath);

// Update value
PlistElementDict rootDict = plist.root;
rootDict.SetString("NSMicrophoneUsageDescription", "Allow access to microphone");
rootDict.SetString("NSPhotoLibraryUsageDescription", "Please attach image.");

// Write plist
File.WriteAllText(plistPath, plist.WriteToString());

}
 
private static void CopyAndReplaceDirectory (string srcPath, string dstPath)
{
if (Directory.Exists (dstPath))
Directory.Delete (dstPath);
if (File.Exists (dstPath))
File.Delete (dstPath);
 
Directory.CreateDirectory (dstPath);
 
foreach (var file in Directory.GetFiles(srcPath))
File.Copy (file, Path.Combine (dstPath, Path.GetFileName (file)));
 
foreach (var dir in Directory.GetDirectories(srcPath))
CopyAndReplaceDirectory (dir, Path.Combine (dstPath, Path.GetFileName (dir)));
}

}
#endif