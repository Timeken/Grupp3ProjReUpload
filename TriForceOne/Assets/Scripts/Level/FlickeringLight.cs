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
            sparks.Stop(true);
            sparks.Play(true);
            
            flash.enabled = true;
            yield return new WaitForSeconds(Random.Range(0.1f, 0.2f));
            flash.enabled = false;
            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
        }
    }	
}
