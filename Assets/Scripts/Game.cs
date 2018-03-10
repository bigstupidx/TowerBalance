using UnityEngine;
using System.Collections;
using System;

public static class Game
{
    public static bool Lost { get; set; }

    public static int Score { get; private set; }

    public static bool IsPaused { get; private set; }

    static Game()
    {
        Lost = false;
        Score = 0;
    }

    public static void AddPoint()
    {
        ++Score;
    }

    public static bool IsNewHighScore()
    {
        bool newScore = false;

        if(Score == PlayerPrefs.GetInt("HighScore", 0) + 1)
        {
            newScore = true;
        }

        return newScore;
    }

    public static void ResetScore()
    {
        Score = 0;
    }

    public static void Pause()
    {
        if(!IsPaused)
        {
            IsPaused = true;

            foreach(GameObject gObject in GameObject.FindObjectsOfType<GameObject>())
            {
                IPausible pausibleObject = gObject.GetComponent<IPausible>();

                if(pausibleObject != null)
                {
                    pausibleObject.Pause();
                }
            }
        }
    }

    public static void Resume()
    {
        if(IsPaused)
        {
            IsPaused = false;

            foreach(GameObject gObject in GameObject.FindObjectsOfType<GameObject>())
            {
                IPausible pausibleObject = gObject.GetComponent<IPausible>();

                if(pausibleObject != null)
                {
                    pausibleObject.Resume();
                }
            }
        }
    }
}
