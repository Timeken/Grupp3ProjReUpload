using UnityEngine;
using UnityEngine.AI;
using System.Collections;


public class Enemy : MonoBehaviour
{
	public int health;
	public int score;
	public int scrapValue;
	public float scrapTimer;
	public float maxSpeed;
	public float deathTimer;
	public bool dead;
	public GameObject[] scrapArr;

	GameObject gameManager;
	GameObject target;
	public bool targetable = false;
	bool finished;
	bool slowed;
	float slowedAmount;
	int magnets;
	Animator anim;
	AudioSource sound;
	EnemyManager enemyManager;

	void Start()
	{
		NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent>();
		if (navMeshAgent.enabled && !navMeshAgent.isOnNavMesh)
		{
			Vector3 position = transform.position;
			NavMeshHit hit;
			NavMesh.SamplePosition(position, out hit, 10.0f,  NavMesh.AllAreas);
			position = hit.position; // usually this barely changes, if at all
			navMeshAgent.Warp(position);
		}

		enemyManager = GameObject.Find("GameManager").GetComponent<EnemyManager>();
		sound = GetComponent<AudioSource>();
		anim = GetComponent<Animator>();
		magnets = 0;
		navMeshAgent.speed = maxSpeed;
		dead = false;
		finished = false;
		gameManager = GameObject.Find("GameManager");
		target = GameObject.FindGameObjectWithTag("Target");
		navMeshAgent.SetDestination(target.transform.position);
	}

	public void TakeDamage(int damage)
	{
		health -= damage;
		if (health <= 0 && !dead)
		{
			Death();
		}
	}


	void Death()
	{
		anim.SetTrigger("death");
		if (sound)
		{
			sound.Play();
		}

		enemyManager.RemoveEnemy(this.gameObject);
		dead = true;
		Destroy(GetComponent<UnityEngine.AI.NavMeshAgent>());
		gameManager.GetComponent<ScoreManager>().Score = score;
		GetComponent<BoxCollider>().enabled = false;
		Destroy(GetComponent<Rigidbody>());
		gameManager.GetComponent<ScrapManager>().ScrapChange(scrapValue);
		gameManager.GetComponent<SpawnManager>().EnemyDead();
		gameManager.GetComponent<EnemyManager>().RemoveEnemy(this.gameObject);
		StartCoroutine("Destruction");
	}


	public void Despawn()
	{
		StartCoroutine("Destruction");
	}

	public int Health
	{
		get { return health; }
		set { health = value; }
	}

	IEnumerator Destruction()
	{
		if (dead)
		{
			SpawnScrap();
		}
		yield return new WaitForSeconds(deathTimer);
		Destroy(this.gameObject);
	}

	void SpawnScrap()
	{

		Quaternion rotOffset = Quaternion.Euler(this.transform.rotation.x, this.transform.rotation.y + Random.Range(-90f, 90f), this.transform.rotation.z + Random.Range(-70f, 70f));
		Vector3 posOffset = new Vector3(Random.Range(-1f, 1f), -this.transform.position.y, Random.Range(-1f, 1f));
		GameObject scrap = Instantiate(scrapArr[Random.Range(0, scrapArr.Length)], transform.position + posOffset, rotOffset) as GameObject;
		scrap.GetComponent<ScrapScript>().ScrapAmount = scrapValue/3;
	}

	public void SetFinished()
	{
		finished = true;
	}

	public bool GetFinished()
	{
		return finished;
	}

	public bool GetDead()
	{
		return dead;
	}

	public bool GetSlowed()
	{
		return slowed;
	}

	public float GetSlowedAmount()
	{
		return slowedAmount;
	}

	public void Slow(bool slowing, float strength)
	{
		this.slowed = slowing;
		if (strength > 0)
		{
			this.slowedAmount = 1 - (strength / 100);
			GetComponent<UnityEngine.AI.NavMeshAgent>().speed = maxSpeed * slowedAmount;
		}
		else if (strength <= 0)
		{
			GetComponent<UnityEngine.AI.NavMeshAgent>().speed = maxSpeed;
		}
	}

	public int GetMagnets()
	{
		return magnets;
	}

	public void SetMagnets(int amount)
	{
		magnets += amount;
	}

	public void ToggleTargetable()
	{
		targetable = !targetable;
	}
}