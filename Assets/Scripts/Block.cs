using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour
{
    private AudioSource m_BrickHitSound;

    // Use this for initialization
    void Start()
    {
        m_BrickHitSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D i_Collision)
    {
        if(!m_BrickHitSound.isPlaying)
        {
            m_BrickHitSound.Play();
        }
    }
}
