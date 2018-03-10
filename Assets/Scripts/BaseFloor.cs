using UnityEngine;
using System.Collections;
using System;

public class BaseFloor : MonoBehaviour
{
    private const string k_BlockTag = "Block";

    private int m_BlocksTouchingTheFloor = 0;
    [SerializeField]
    private int m_LoseThreshold = 0;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnCollisionEnter2D(Collision2D i_Collision)
    {
        if (i_Collision.collider.CompareTag(k_BlockTag))
        {
            ++m_BlocksTouchingTheFloor;

            if (m_BlocksTouchingTheFloor > m_LoseThreshold)
            {
                StartCoroutine(Camera.main.GetComponent<BlockPlacer>().LostGame());
            }
        }
    }

    void OnTriggerEnter2D(Collider2D i_Collider)
    {
        if (i_Collider.CompareTag(k_BlockTag))
        {
            ++m_BlocksTouchingTheFloor;

            if (m_BlocksTouchingTheFloor > m_LoseThreshold)
            {
                StartCoroutine(Camera.main.GetComponent<BlockPlacer>().LostGame());
            }
        }
    }

    void OnCollisionExit2D(Collision2D i_Collision)
    {
        if (i_Collision.collider.CompareTag(k_BlockTag))
        {
            --m_BlocksTouchingTheFloor;
        }
    }

    void OnColliderExit2D(Collider2D i_Collider)
    {
        if (i_Collider.CompareTag(k_BlockTag))
        {
            --m_BlocksTouchingTheFloor;
        }
    }
}
