using UnityEngine;
using System.Collections;

//using GooglePlayGames;
//using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

public class Menu : MonoBehaviour
{
	private const string k_GameScene = "TowerBuildingStage";
	private const string k_CreditsScene = "Credits";
	private const string k_MainMenuScene = "MainMenu";

	private AudioSource m_MenuButtonSound;

	// Use this for initialization
	void Start()
	{
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		m_MenuButtonSound = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit();
		}
	}

	public void StartGame()
	{
		m_MenuButtonSound.Play();
		Game.Lost = false;
		Game.ResetScore();
		Game.Resume();
		Application.LoadLevel(k_GameScene);
	}

	public void HighScores()
	{
		m_MenuButtonSound.Play();
		if (!Social.localUser.authenticated) {
//            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
//            PlayGamesPlatform.InitializeInstance(config);
//            PlayGamesPlatform.Activate();

			Social.localUser.Authenticate((success) => {
				if (success) {
					Social.ReportScore(PlayerPrefs.GetInt("HighScore", 0), "CgkIrsrB6OwBEAIQAA", (s) => {
					});
					Social.ShowLeaderboardUI();
				}
			});
		} else {
			Social.ReportScore(PlayerPrefs.GetInt("HighScore", 0), "CgkIrsrB6OwBEAIQAA", (s) => {
			});
			Social.ShowLeaderboardUI();
		}
	}

	public void Credits()
	{
		m_MenuButtonSound.Play();
		Application.LoadLevel(k_CreditsScene);
	}

	public void MainMenu()
	{
		m_MenuButtonSound.Play();
		Application.LoadLevel(k_MainMenuScene);
	}
}
