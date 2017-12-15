using UnityEngine;
using System.Collections;

public class TurretTower : CombatTower {

	public TowerUpgrade[] damageUpgrade = new TowerUpgrade[3];
	public TowerUpgrade[] reloadUpgrade = new TowerUpgrade[3];
	public TowerUpgrade[] rangeUpgrade = new TowerUpgrade[3];

	public GameObject[] bulletSpawns;
	// Use this for initialization

	new void Start()
	{
		base.Start();

		this.currentValues[0] = damage;
		currentValues[1] = maxAmmo;
		currentValues[2] = range;

		upgrades[0] = damageUpgrade;
		upgrades[1] = reloadUpgrade;
		upgrades[2] = rangeUpgrade;

		upgradeNames[0] = "damage";
		upgradeNames[1] = "magazine size";
		upgradeNames[2] = "range";
    }

	protected override void Fire()
	{
		base.Fire();

		thisShot = shot as GameObject;
		thisShot.GetComponent<Shot>().SetTarget(target);
		thisShot.GetComponent<Shot>().damage = this.damage;

		Instantiate(shot, bulletSpawns[0].transform.position, Quaternion.identity);
		Instantiate(shot, bulletSpawns[1].transform.position, Quaternion.identity);

		shotTimer = Time.time;
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
			maxAmmo += upgrades[upgradeIndex][currentUpgrades[upgradeIndex]].upgrade;
			currentValues[1] = maxAmmo;
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
