using UnityEngine;
using System.Collections;

public class Magnet : TowerScript
{
	public float strength;
	public GameObject[] effects;
	public int scrapIncrease;

	public TowerUpgrade[] slowUpgrade = new TowerUpgrade[3];
	public TowerUpgrade[] increaseUpgrade = new TowerUpgrade[3];
	public TowerUpgrade[] rangeUpgrade = new TowerUpgrade[3];

	float r;
	bool canActivate;
	Vector3 center;
	AudioSource sound;
	SphereCollider area;

	void Start()
	{
		area = GetComponent<SphereCollider>();
		sound = GetComponent<AudioSource>();
		transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
		canActivate = true;
		center = transform.position;
		r = GetComponent<SphereCollider>().radius;
		range = (int)GetComponent<SphereCollider>().radius;


		currentValues[0] = strength;
		currentValues[1] = scrapIncrease;
		currentValues[2] = range;

		upgrades[0] = slowUpgrade;
		upgrades[1] = increaseUpgrade;
		upgrades[2] = rangeUpgrade;

		upgradeNames[0] = "slow amount";
		upgradeNames[1] = "scrap value";
		upgradeNames[2] = "range";

		foreach (GameObject effect in effects)
		{
			if (effect.name == "AreaOfEffect")
			{
				effect.transform.localScale = new Vector3(0.2f * range, 0.2f * range, 0.2f * range);
			}
		}
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.CompareTag("Enemy") && col.GetComponent<Enemy>() != null)
		{
			col.GetComponent<Enemy>().SetMagnets(1);
			if (col.GetComponent<Enemy>().GetSlowed() == false || col.GetComponent<Enemy>().GetSlowedAmount() < strength)
			{
				col.GetComponent<Enemy>().Slow(true, strength);
			}
		}
		else if (col.gameObject.CompareTag("Scrap") && col.GetComponent<ScrapScript>() != null && strength > col.GetComponent<ScrapScript>().GetCurrentPull())
		{
			col.GetComponent<ScrapScript>().Activate(transform, strength);
		}
	}

	void OnTriggerExit(Collider col)
	{
		if (col.gameObject != null && col.gameObject.CompareTag("Enemy"))
		{
			col.GetComponent<Enemy>().Slow(false, 0);
		}
	}

	void Update()
	{

	}


	void Unslow()
	{
		Collider[] hitColliders = Physics.OverlapSphere(center, r);
		foreach (Collider coll in hitColliders)
		{
			if (coll.gameObject.CompareTag("Enemy") && coll.gameObject.GetComponent<Enemy>() != null)
			{
				coll.gameObject.GetComponent<Enemy>().SetMagnets(-1);
				if (coll.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>() != null && coll.gameObject.GetComponent<Enemy>().GetMagnets() <= 0)
				{
					coll.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().speed = coll.gameObject.GetComponent<Enemy>().maxSpeed;
				}
			}
		}
	}

	void OnDestroy()
	{
		Unslow();
	}

	public void Upgrade(int upgradeIndex)
	{
		if (upgradeIndex == 0)
		{
			strength += upgrades[upgradeIndex][currentUpgrades[upgradeIndex]].upgrade;
			currentValues[0] = strength;
		}
		else if (upgradeIndex == 1)
		{
			scrapIncrease += upgrades[upgradeIndex][currentUpgrades[upgradeIndex]].upgrade;
			currentValues[1] = scrapIncrease;
		}
		else
		{
			GetComponent<SphereCollider>().radius += upgrades[upgradeIndex][currentUpgrades[upgradeIndex]].upgrade;
			range += upgrades[upgradeIndex][currentUpgrades[upgradeIndex]].upgrade;
			currentValues[2] = GetComponent<SphereCollider>().radius;
			foreach (GameObject effect in effects)
			{
				if (effect.name == "AreaOfEffect")
				{
					effect.transform.localScale = new Vector3(0.2f * range, 0.2f * range, 0.2f * range);
                }
			}
		}

		sellValue += upgrades[upgradeIndex][currentUpgrades[upgradeIndex]].value;
		currentUpgrades[upgradeIndex]++;
	}
}