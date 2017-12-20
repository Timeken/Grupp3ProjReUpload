using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DespawnManager : MonoBehaviour
{
    GameObject gm;

    void Start()
    {
        gm = GameObject.Find("GameManager");
    }

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Enemy") && other.GetComponent<Enemy>().GetFinished() == false)
		{
			other.GetComponent<Enemy>().SetFinished();
			other.GetComponent<Enemy>().Despawn();
			gm.GetComponent<GameOverManager>().LoseLife();
            gm.GetComponent<SpawnManager>().EnemyDead();
            GameObject.Find("EnemyGotThroughSound").GetComponent<AudioSource>().Play();
		}
	}
}
