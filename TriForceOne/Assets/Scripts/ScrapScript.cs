using UnityEngine;
using System.Collections;

public class ScrapScript : MonoBehaviour
{

    public int scrapAmount;
    public float despawnTime;
    public Transform pullingMagnet;

    float size = 1;
    float distance;
    float step;
    float currentPull;

    void Update()
    {
        if (pullingMagnet != null)
        {
            MoveAndScale();
        }
    }

    void Start()
    {
        StartCoroutine("ScrapDestroy");
    }

    void MoveAndScale()
    {
		if (Vector3.Distance(pullingMagnet.position, transform.position) <= 0.5f)
		{
			GameObject.Find("GameManager").GetComponent<ScrapManager>().ScrapChange(scrapAmount * pullingMagnet.GetComponent<Magnet>().scrapIncrease);
			Destroy(this.gameObject);
		}
        size = Vector3.Distance(pullingMagnet.position, transform.position) / distance;
        transform.localScale = new Vector3(size, size, size);

        step = currentPull * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, pullingMagnet.position, step);
    }

    public int ScrapAmount
    {
        get { return scrapAmount; }
        set { scrapAmount = value; }
    }

    public void Activate(Transform pullingMagnet, float strength)
    {
        this.currentPull = strength;
        if (strength > 0)
        {
            this.pullingMagnet = pullingMagnet;
        }
        else if (strength <= 0)
        {
            this.pullingMagnet = null;
        }
        distance = Vector3.Distance(pullingMagnet.position, transform.position);
    }

    public float GetCurrentPull()
    {
        return currentPull;
    }

    IEnumerator ScrapDestroy()
    {
        yield return new WaitForSeconds(despawnTime);
        Destroy(this.gameObject);
    }
}
