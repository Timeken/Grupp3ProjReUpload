using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScrapManager : MonoBehaviour
{
    public float flashTime;
    public int startingScrap;

    static int scrap;
    Text text;

    void Start()
    {
        scrap = startingScrap;
        text = GameObject.Find("ScrapText").GetComponent<Text>();
        text.text = "Scrap: " + scrap;
    }

    public bool ScrapChange(int value)
    {
        bool returnValue = false;

        if (value < 0 && scrap < Mathf.Abs(value))
        {
            StartCoroutine("InsufficientScrap");
            returnValue = false;
        }
        else
        {
            scrap += value;
            returnValue = true;
        }
        text.text = "Scrap: " + scrap;
        return returnValue;
    }

    public int GetScrap()
    {
        return scrap;
    }

    IEnumerator InsufficientScrap()
    {
        GameObject.Find("InvalidBuildSound").GetComponent<AudioSource>().Play();
        GameObject.Find("InsufficientScrap").GetComponent<Text>().enabled = true;
        yield return new WaitForSeconds(flashTime);
        GameObject.Find("InsufficientScrap").GetComponent<Text>().enabled = false;
    }
}