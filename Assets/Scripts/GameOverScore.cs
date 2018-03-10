using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameOverScore : MonoBehaviour
{
    private const string k_ScoreTextTag = "ScoreText";
    private const string k_HighScoreTextTag = "HighScoreText";

    // Use this for initialization
    void Start()
    {
        Text gameScore = GameObject.FindGameObjectWithTag(k_ScoreTextTag).GetComponent<Text>();
        int score = int.Parse(gameScore.text);
        GetComponent<Text>().text = gameScore.text;
        Destroy(gameScore.transform.root.gameObject);

        if(score > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", score);
        }

        GameObject.FindGameObjectWithTag(k_HighScoreTextTag).GetComponent<Text>().text = PlayerPrefs.GetInt("HighScore", 0).ToString();
    }
}
