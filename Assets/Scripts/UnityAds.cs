using System.Collections;
using UnityEngine;
//using UnityEngine.Advertisements;

public class UnityAds : MonoBehaviour
{
	#if (UNITY_ANDROID || UNITY_IOS) && UNITY_5_6
	private void Start()
	{
//		if (!Advertisement.isInitialized) {
//#if UNITY_ANDROID
//            Advertisement.Initialize("1004195");
//#elif UNITY_IOS
//			Advertisement.Initialize("1004196");
//#endif
//		}

//		StartCoroutine(ShowAd());
//	}
//
//	private void Update()
//	{
//	}
//
//	public IEnumerator ShowAd()
//	{
//		while (!Advertisement.IsReady()) {
//			yield return null;
//		}
//
//		Game.Pause();
//		Advertisement.Show();
//
//		while (Advertisement.isShowing) {
//			yield return null;
//		}
//
//		Game.Resume();
	}
	#endif
}