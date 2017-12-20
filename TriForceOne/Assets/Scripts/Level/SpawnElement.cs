using UnityEngine;
using System.Collections;

[System.Serializable]
public class SpawnElement
{
	public GameObject enemy;
	public int count;
	public float delay;
	public int health;

}

//class med en array av SpawnElements så att man kan skapa olika antal waves för varje bana. - Alexander W
[System.Serializable]
public class SpawnList
{
    public SpawnElement[] wave;
}