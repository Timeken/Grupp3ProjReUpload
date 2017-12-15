using UnityEngine;
using System.Collections;

public class MissileTowerScript : CombatTower
{

	public GameObject[] missileSpawns;
	bool missileAlternator = false;
	public float blastRadius;

	public TowerUpgrade[] damageUpgrade = new TowerUpgrade[3];
	public TowerUpgrade[] blastUpgrade = new TowerUpgrade[3];
	public TowerUpgrade[] rangeUpgrade = new TowerUpgrade[3];

	// Use this for initialization
	new void Start()
	{
		base.Start();

		currentValues[0] = damage;
		currentValues[1] = blastRadius;
		currentValues[2] = range;

		upgrades[0] = damageUpgrade;
		upgrades[1] = blastUpgrade;
		upgrades[2] = rangeUpgrade;

		upgradeNames[0] = "damage";
		upgradeNames[1] = "blast radius";
		upgradeNames[2] = "range";

	}

	protected override void Fire()
	{
		if (missileAlternator)
			bulletSpawn = missileSpawns[0];

		else
			bulletSpawn = missileSpawns[0];

		missileAlternator = !missileAlternator;

		thisShot = shot as GameObject;
		thisShot.GetComponent<Shot>().SetTarget(target);
		thisShot.GetComponent<Shot>().damage = this.damage;
		thisShot.GetComponent<Shot>().blastRadius = this.blastRadius;

		Instantiate(shot, bulletSpawn.transform.position, Quaternion.identity);

		shotTimer = Time.time;
	}
	
	public void Upgrade(int upgradeIndex)
	{
		if (upgradeIndex == 0)
		{
			damage += upgrades[upgradeIndex][currentUpgrades[upgradeIndex]].upgrade;
			currentValues[0] = damage;
		}
		else if (upgradeIndex == 1)
		{
			blastRadius += upgrades[upgradeIndex][currentUpgrades[upgradeIndex]].upgrade;
			currentValues[1] = blastRadius;
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
