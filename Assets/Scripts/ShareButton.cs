using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShareButton : MonoBehaviour
{
    public  Button shareButton;

	private bool isFocus = false;
	private bool isProcessing = false;

	void  Start () {
		shareButton.onClick.AddListener (ShareText);
	}

	void OnApplicationFocus (bool focus) {
		isFocus = focus;
	}

	private void ShareText () {

		#if UNITY_ANDROID
		if (!isProcessing) {
			StartCoroutine (ShareTextInAnroid ());
		}
		#else
		Debug.Log("No sharing set up for this platform.");
		#endif
	}



	#if UNITY_ANDROID
	public IEnumerator ShareTextInAnroid () {

		var shareSubject = "I challenge you to beat me in jigsaw puzzle game on the Jodo app";
		var shareMessage = "I challenge you to beat me in jigsaw puzzle game on the Jodo app. \n" +
		                   "Please use the below code to join the game. \n" +
		                   "Game Code: "+ Launcher.instance.GetRoomName() + "\n\n"
                           +
                           "Download the app: \n" +
						   "Android: \n"+
						   "iOS: ";

		isProcessing = true;

		if (!Application.isEditor) {
			//Create intent for action send
			AndroidJavaClass intentClass = 
				new AndroidJavaClass ("android.content.Intent");
			AndroidJavaObject intentObject = 
				new AndroidJavaObject ("android.content.Intent");
			intentObject.Call<AndroidJavaObject> 
				("setAction", intentClass.GetStatic<string> ("ACTION_SEND"));

			//put text and subject extra
			intentObject.Call<AndroidJavaObject> ("setType", "text/plain");
			intentObject.Call<AndroidJavaObject> 
				("putExtra", intentClass.GetStatic<string> ("EXTRA_SUBJECT"), shareSubject);
			intentObject.Call<AndroidJavaObject> 
				("putExtra", intentClass.GetStatic<string> ("EXTRA_TEXT"), shareMessage);

			//call createChooser method of activity class
			AndroidJavaClass unity = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
			AndroidJavaObject currentActivity = 
				unity.GetStatic<AndroidJavaObject> ("currentActivity");
			AndroidJavaObject chooser = 
				intentClass.CallStatic<AndroidJavaObject> 
				("createChooser", intentObject, "Share your high score");
			currentActivity.Call ("startActivity", chooser);
		}

		yield return new WaitUntil (() => isFocus);
		isProcessing = false;
	}
	#endif
}
