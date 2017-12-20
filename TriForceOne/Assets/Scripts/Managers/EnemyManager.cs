using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour {

	static List<GameObject> enemies = new List<GameObject>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void AddEnemy(GameObject enemy)
	{
		enemies.Add(enemy);
	}

	public void RemoveEnemy(GameObject enemy)
	{
		enemies.Remove(enemy);
	}

	public List<GameObject> Enemies
	{
		get { return enemies; }
	}
}
