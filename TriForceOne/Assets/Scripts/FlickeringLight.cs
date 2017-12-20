using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour {

    private Light flash;
    [SerializeField]
    private float minWaitTime, maxWaitTime;
    [SerializeField]
    private ParticleSystem sparks;

    void Start()
    {
        flash = GetComponent<Light>();
        StartCoroutine(Flicker());
    }

    IEnumerator Flicker()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
            sparks.Stop(true);
            sparks.Play(true);
            flash.enabled = !flash.enabled;
        }
    }	
}
