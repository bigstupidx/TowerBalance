using UnityEngine;
using System.Collections;

public class DontDestroyOnLoad : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(transform.root.gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
