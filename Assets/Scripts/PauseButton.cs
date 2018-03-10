using UnityEngine;
using System.Collections;

public class PauseButton : MonoBehaviour
{
    public bool Paused { get; set; }

    private AudioSource m_MenuButtonSound;

    [SerializeField]
    public GameObject m_PauseMenu;

    // Use this for initialization
    void Start()
    {
        m_MenuButtonSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TriggerPause()
    {
        m_MenuButtonSound.Play();

        if (Paused)
        {
            Paused = false;
            m_PauseMenu.SetActive(false);
            Game.Resume();
        }
        else
        {
            Paused = true;
            m_PauseMenu.SetActive(true);
            Game.Pause();
        }
    }
}
