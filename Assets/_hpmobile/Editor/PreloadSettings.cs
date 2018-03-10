using UnityEngine;
using UnityEditor;
using System.IO;

[InitializeOnLoad]
public class PreloadSettings : MonoBehaviour
{
	static PreloadSettings()
	{
		EditorSettings.externalVersionControl = ExternalVersionControl.Generic;
//		if (EditorSettings.serializationMode != SerializationMode.ForceText)
//			EditorSettings.serializationMode = SerializationMode.ForceText;

		if (PlayerSettings.displayResolutionDialog != ResolutionDialogSetting.HiddenByDefault)
			PlayerSettings.displayResolutionDialog = ResolutionDialogSetting.HiddenByDefault;

		PlayerSettings.Android.preferredInstallLocation = AndroidPreferredInstallLocation.PreferExternal;
		PlayerSettings.accelerometerFrequency = 60;
		PlayerSettings.defaultIsFullScreen = true;
		PlayerSettings.statusBarHidden = true;
		PlayerSettings.stripUnusedMeshComponents = true;
		PlayerSettings.strippingLevel = StrippingLevel.UseMicroMSCorlib;

		PlayerSettings.companyName = "HappyMobile";

		#if UNITY_4_7
		PlayerSettings.Android.targetDevice = AndroidTargetDevice.FAT;
		#endif

		#if UNITY_5_5_OR_NEWER
		PlayerSettings.stripEngineCode = true;
		#endif

		if (PlayerSettings.apiCompatibilityLevel != ApiCompatibilityLevel.NET_2_0_Subset)
			PlayerSettings.apiCompatibilityLevel = ApiCompatibilityLevel.NET_2_0_Subset;

		Debug.Log("Auto Set Project Settings Finish!");
		SetAndroidSettings();
		SetIOSSettings();
	}

	static void SetAndroidSettings()
	{
		#if UNITY_5_5_OR_NEWER
		PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.Android, true);
		PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.Mono2x);
		PlayerSettings.Android.androidIsGame = true;
		PlayerSettings.Android.androidTVCompatibility = true;
		#endif
		PlayerSettings.Android.preferredInstallLocation = AndroidPreferredInstallLocation.PreferExternal;
		QualitySettings.SetQualityLevel(5, true);
	}

	static void SetIOSSettings()
	{
		PlayerSettings.iOS.targetDevice = iOSTargetDevice.iPhoneAndiPad;
		PlayerSettings.iOS.sdkVersion = iOSSdkVersion.DeviceSDK;
		#if UNITY_5_5_OR_NEWER
		// 0 - None, 1 - ARM64, 2 - Universal.
		PlayerSettings.SetArchitecture(BuildTargetGroup.iOS, 2);
		PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.iOS, true);
		PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, ScriptingImplementation.IL2CPP);
		PlayerSettings.iOS.allowHTTPDownload = true;
		PlayerSettings.iOS.appInBackgroundBehavior = iOSAppInBackgroundBehavior.Suspend;
		PlayerSettings.iOS.cameraUsageDescription = "Camera";
		PlayerSettings.iOS.locationUsageDescription = "Location";
		PlayerSettings.iOS.microphoneUsageDescription = "Microphone";
		#endif
	}
}
	