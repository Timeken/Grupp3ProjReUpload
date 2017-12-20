using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TowerScript : MonoBehaviour
{
	public int sellValue;
	public int scrapCost;
	public int range;
	
	public int[] currentUpgrades = new int[] { 0, 0, 0 };

	public TowerUpgrade[][] upgrades = new TowerUpgrade[3][];
	protected string[] upgradeNames = new string[3];
	protected float[] currentValues = new float[3];

	GameObject foundation;

    public GameObject Foundation
    {
        get { return foundation; }
        set { foundation = value; }
    }
    
	public bool AvailableUpgrade(int upgradeIndex)
	{
		if (currentUpgrades[upgradeIndex] < upgrades[upgradeIndex].Length)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public int SellValue
	{
		get { return this.sellValue; }
	}

	public int GetUpgradeCost(int upgradeIndex)
	{
		int upgradeCost = this.upgrades[upgradeIndex][currentUpgrades[upgradeIndex]].cost;

		return upgradeCost;
	}

	public string UpgradeName(int index)
	{
		return upgradeNames[index];
	}

	public float CurrentValue(int index)
	{
		return currentValues[index];
	}

	public int UpgradeValues(int index)
	{
		return this.upgrades[index][currentUpgrades[index]].upgrade;
	}
}
