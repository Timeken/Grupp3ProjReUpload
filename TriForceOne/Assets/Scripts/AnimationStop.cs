using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStop : MonoBehaviour {

    private bool stop = false;
    private Animator ani;

    private void StopAni()
    {
        stop = false;
        ani.enabled = stop;
    }
    public void StartAni()
    {
        stop = true;
    }

    private void Start()
    {
        ani = GetComponent<Animator>();
        ani.enabled = stop;
    }

    private void Update()
    {
        if (stop)
        {
            ani.enabled = stop;
        }
    }
}
