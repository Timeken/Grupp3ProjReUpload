using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SetUpScript : MonoBehaviour
{
	public GameObject marker;
	public GameObject foundation;
	public GameObject gameManager;
	public GameObject trap;
	public int numberOfTiles;
	public int numberOfTraps;
	int rand;

	// Use this for initialization
	void Start()
	{
		//Sätter igång tiden varje gång nivån laddas in. Detta för att förhindra ev. problem vid pauser och omstart av spelet
		Time.timeScale = 1;
		marker = GameObject.Find("FoundationMarker");

		//Skapar en foundation baserat på antal rutor definierat i editorn
		for (int i = 0; i < numberOfTiles; i++)
		{
			for (int o = 0; o < numberOfTiles; o++)
			{
				//Ger varje foundation en plats, en referens till markören samt till gamemanager
				//GameObject g = foundation as GameObject;
                GameObject g = Instantiate(foundation, transform, true);
                g.transform.position = new Vector3(-numberOfTiles / 2 + i + 0.5f+ marker.transform.position.x, -0.4f , -numberOfTiles / 2 + o + 0.5f + marker.transform.position.z);
                g.GetComponent<FoundationScript>().marker = this.marker;
				g.GetComponent<FoundationScript>().gameManager = this.gameManager;
				
			}
		}

		//Hämtar alla foundations och ger dem traps, instantierar även dessa på korrekt plats
		GameObject[] foundations = GameObject.FindGameObjectsWithTag("Foundation");
		for (int i = 0; i < numberOfTraps; i++)
		{
			rand = Random.Range(0, foundations.Length);
			if (!foundations[rand].GetComponent<FoundationScript>().hasTrap)
			{
				foundations[rand].GetComponent<FoundationScript>().AttachTrap(trap);
			}
			else
				i--;


		}
	}
}
