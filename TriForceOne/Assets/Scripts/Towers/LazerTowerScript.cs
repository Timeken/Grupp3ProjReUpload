using UnityEngine;
using System.Collections;

public class LazerTowerScript : CombatTower
{
	public GameObject[] firePoints;
	Transform rayOrigin;

	Ray ray;
	RaycastHit hit;

	public TowerUpgrade[] damageUpgrade = new TowerUpgrade[3];
	public TowerUpgrade[] fireRateUpgrade = new TowerUpgrade[3];
	public TowerUpgrade[] rangeUpgrade = new TowerUpgrade[3];

	protected GameObject verticalRotator;

	protected Quaternion verticalRotation;

	LineRenderer beam;
	int shotIndex = 0;

	public LayerMask layerMask;

	new void Start()
	{
		base.Start();

		beam = GetComponent<LineRenderer>();

		foreach (Transform t in GetComponentsInChildren<Transform>())
		{
			if (t.name == "VerticalRotator")
			{
				rayOrigin = t;
				verticalRotator = t.gameObject;
				break;
			}
		}
		currentValues[0] = damage;
		currentValues[1] = timeBetweenShots;
		currentValues[2] = range;

		upgrades[0] = damageUpgrade;
		upgrades[1] = fireRateUpgrade;
		upgrades[2] = rangeUpgrade;

		upgradeNames[0] = "damage";
		upgradeNames[1] = "fire rate";
		upgradeNames[2] = "range";

	}

	new void Update()
	{
		base.Update();

		Debug.DrawRay(rayOrigin.transform.position, rayOrigin.forward * 100, Color.cyan);

		if (target)
		{
			verticalRotator.transform.LookAt(target.transform);			
		}
	}

	protected override void Fire()
	{
		ray = new Ray(rayOrigin.position, rayOrigin.forward);

		if (Physics.Raycast(ray, out hit, range, layerMask))
		{
			if (hit.collider.gameObject.CompareTag("Enemy"))
			{
				beam.SetPosition(1, hit.point);
				beam.SetPosition(0, firePoints[shotIndex].transform.position);

				hit.collider.gameObject.GetComponent<Enemy>().TakeDamage(damage);

				if (shotIndex <= 2)
				{
					shotIndex++;
				}
				else
				{
					shotIndex = 0;
				}
				StartCoroutine("FireBeam");

				firePoints[shotIndex].GetComponentInParent<Animator>().SetTrigger("fire");
				ammo--;
				shotTimer = Time.time;
			}

			else
			{
				return;
			}
		}
	}

	IEnumerator FireBeam()
	{
		GetComponent<AudioSource>().pitch = 1 + Random.Range(-0.5f, 0.5f);
		GetComponent<AudioSource>().Play();
		beam.enabled = true;
		yield return new WaitForSeconds(0.1f);
		beam.enabled = false;
	}

	//FIXME lägg in relevanta namn och länka till rätt variabler
	public void Upgrade(int upgradeIndex)
	{
		if (upgradeIndex == 0)
		{
			damage += upgrades[upgradeIndex][currentUpgrades[upgradeIndex]].upgrade;
			currentValues[0] = damage;
		}
		else if (upgradeIndex == 1)
		{
			timeBetweenShots -= (upgrades[upgradeIndex][currentUpgrades[upgradeIndex]].upgrade / 100f);
			currentValues[1] = timeBetweenShots;
		}
		else
		{
			range += upgrades[upgradeIndex][currentUpgrades[upgradeIndex]].upgrade;
			currentValues[2] = range;
		}

		sellValue += upgrades[upgradeIndex][currentUpgrades[upgradeIndex]].value;
		currentUpgrades[upgradeIndex]++;
	}
}