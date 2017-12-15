using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FoundationMarkerScript : MonoBehaviour
{
	public Material valid;
	public Material invalid;
	public bool onBoard = false;
	public Projector projector;

	public GameObject heapMenu;
	public GameObject buildMenu;
	public GameObject upgrade1;
	public GameObject upgrade2;
	public GameObject upgrade3;
	public GameObject upgradeMenu;

	GameObject foundation;

	public GameObject[] menus;
	public GameObject[] upgrades;

	void Start()
	{
		upgrades = new GameObject[3];
		upgrades[0] = upgrade1;
		upgrades[1] = upgrade2;
		upgrades[2] = upgrade3;
		menus = new GameObject[3];
		menus[0] = heapMenu;
		menus[1] = buildMenu;
		menus[2] = upgradeMenu;
		projector.orthographicSize = 0;
	}

	public void MoveMarker(GameObject foundation)
	{
		this.foundation = foundation;

		projector.orthographicSize = 0;

		foreach (GameObject menu in menus)
		{
			menu.SetActive(false);
		}

		onBoard = true;

		if (foundation.GetComponent<FoundationScript>().valid)
		{
			gameObject.GetComponent<MeshRenderer>().material = valid;
		}
		else
		{
			gameObject.GetComponent<MeshRenderer>().material = invalid;
		}

		Vector3 offset = new Vector3(0, 0.5f, 0);

		if (foundation.GetComponent<FoundationScript>().HasTower)
		{
			StartCoroutine("TowerUpgrades");
		}
		else if (foundation.GetComponent<FoundationScript>().HasScrap)
		{
			offset.y += 0.5f;
			buildMenu.SetActive(true);
		}
		else if (foundation.GetComponent<FoundationScript>().hasTrap)
		{
			offset.y += 0.2f;
			heapMenu.SetActive(true);
		}
		else if (foundation.GetComponent<FoundationScript>().valid)
		{
			heapMenu.SetActive(true);
		}

		transform.position = foundation.transform.position + offset;
	}

	public void HideMarker()
	{
		foreach (GameObject menu in menus)
		{
			menu.SetActive(false);
		}
		GameObject.Find("GameManager").GetComponent<BuildingManagerScript>().Reset();
		onBoard = false;
		transform.position = new Vector3(0, 100, 0);
	}

	IEnumerator TowerUpgrades()
	{
		yield return new WaitForSeconds(0);

		upgradeMenu.SetActive(true);

		for (int i = 0; i < upgrades.Length; i++)
		{
			Text[] children = upgrades[i].GetComponentsInChildren<Text>();

			children[0].text = "Upgrade\n" + foundation.GetComponent<FoundationScript>().tower.GetComponent<TowerScript>().UpgradeName(i);
			if (foundation.GetComponent<FoundationScript>().Tower.GetComponent<TowerScript>().AvailableUpgrade(i))
			{
				if (foundation.GetComponent<FoundationScript>().tower.name == "LazerTower(Clone)")
				{
					children[1].text = "Current : " + foundation.GetComponent<FoundationScript>().tower.GetComponent<TowerScript>().CurrentValue(i) + "\n Next: " +
					(foundation.GetComponent<FoundationScript>().tower.GetComponent<TowerScript>().CurrentValue(i) - foundation.GetComponent<FoundationScript>().tower.GetComponent<TowerScript>().UpgradeValues(i)/100f);
				}
				else
				{
					children[1].text = "Current : " + foundation.GetComponent<FoundationScript>().tower.GetComponent<TowerScript>().CurrentValue(i) + "\n Next: " +
					(foundation.GetComponent<FoundationScript>().tower.GetComponent<TowerScript>().CurrentValue(i) + foundation.GetComponent<FoundationScript>().tower.GetComponent<TowerScript>().UpgradeValues(i));
				}
			}
			else
			{
				children[1].text = children[1].text = "Current : " + foundation.GetComponent<FoundationScript>().tower.GetComponent<TowerScript>().CurrentValue(i) + "\n MAXED OUT ";
			}

			if (foundation.GetComponent<FoundationScript>().Tower.GetComponent<TowerScript>().AvailableUpgrade(i))
			{
				children[2].text = "Cost: " + foundation.GetComponent<FoundationScript>().tower.GetComponent<TowerScript>().GetUpgradeCost(i) + " scrap";
			}
			else
			{
				children[2].text = "";
			}
		}
		if(foundation.GetComponent<FoundationScript>().tower.name != "MagnetTower(Clone)")
		//if(foundation.GetComponent<FoundationScript>().tower.GetComponent<CombatTower>())
		{
			projector.orthographicSize = foundation.GetComponent<FoundationScript>().tower.GetComponent<TowerScript>().range / 5;
		}
		else
		projector.orthographicSize = foundation.GetComponent<FoundationScript>().tower.GetComponent<TowerScript>().range;
	}
}
