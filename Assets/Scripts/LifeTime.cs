using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeTime : MonoBehaviour
{
    [Range(0.1f, 10.0f)]
    public float timeToLive = 2.5f;
    public bool startAutomatically = true;

    // Start is called before the first frame update
    void Start()
    {
        if (startAutomatically)
        {
            Invoke(nameof(KillObject), timeToLive);
        }
    }

    public void StartTimer()
    {
        Invoke(nameof(KillObject), timeToLive);
    }

    public void KillObject()
    {
        Destroy(gameObject);
    }
}
