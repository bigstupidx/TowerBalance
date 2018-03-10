using UnityEngine;
using System.Collections;

public interface IPausible
{
    bool Paused { get; set; }

    void Pause();
    void Resume();
}
