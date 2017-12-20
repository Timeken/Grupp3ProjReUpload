using UnityEngine;
using System.Collections;

public class MainMenuSpawner : MonoBehaviour
{

	public GameObject[] enemies;
	public GameObject spawnpoint;

	// Use this for initialization
	void Start()
	{
		StartCoroutine("Spawn");
	}


	IEnumerator Spawn()
	{
		while (true) {
			Instantiate(enemies[Random.Range(0, enemies.GetLength(0))], spawnpoint.transform.position, Quaternion.identity);

			yield return new WaitForSeconds(Random.Range(1.5f, 4));
		}
			
	}
}
