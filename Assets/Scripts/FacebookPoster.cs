using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using Facebook.Unity;
using System.Collections.Generic;

public class FacebookPoster : MonoBehaviour
{
    [SerializeField]
    private string m_GooglePlayPage = "";

    // Use this for initialization
    void Start()
    {
        if (!FB.IsInitialized)
        {
            FB.Init();
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
    
    public void OpenFacebookPrompt()
    {
        if (FB.IsInitialized)
        {
            if (!FB.IsLoggedIn || !AccessToken.CurrentAccessToken.Permissions.Contains("publish_actions"))
            {
                FB.LogInWithPublishPermissions(new List<string>() { "public_profile", "email", "publish_actions" }, fbLoginCallback);
            }
            else
            {
                fbLoginCallback();
            }
        }
    }

    private void fbLoginCallback(ILoginResult result = null)
    {
        if (result == null || string.IsNullOrEmpty(result.Error))
        {
            FB.ShareLink(contentURL: new Uri(m_GooglePlayPage),
                contentTitle: "I just scored on Toewr Balance!",
                contentDescription: "Think you can beat my score?");                
        }
    }
}
