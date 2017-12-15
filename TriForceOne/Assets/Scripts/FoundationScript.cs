using UnityEngine;
using System.Collections;

public class FoundationScript : MonoBehaviour
{
	public bool valid = false;
	public bool clicked = false;

	public bool hasScrapHeap = false;
	public bool hasTower = false;
	public bool hasTrap = false;

	public GameObject scrapHeap;
	public GameObject tower;
	public GameObject trap;

	public GameObject marker;
	public GameObject gameManager;

	public bool HasScrap
	{
		get { return hasScrapHeap; }
		set { hasScrapHeap = value; }
	}

	public bool HasTower
	{
		get { return hasTower; }
		set { hasTower = value; }
	}

	public void ClickedOn(bool showMarker)
	{
		clicked = true;

		if(showMarker)
			marker.GetComponent<FoundationMarkerScript>().MoveMarker(gameObject);
	}

	public void Declick()
	{
		clicked = false;
	}

	public void SellTower()
	{
		gameManager.GetComponent<ScrapManager>().ScrapChange(this.tower.GetComponent<TowerScript>().SellValue);

		Destroy(tower);
		hasTower = false;
		ClickedOn(true);
	}

	public void SellScrap()
	{
		gameManager.GetComponent<ScrapManager>().ScrapChange(gameManager.GetComponent<BuildingManagerScript>().scrapHeapCost);

		Destroy(scrapHeap);

		hasScrapHeap = false;
		ClickedOn(true);
	}

	public void AttachTrap(GameObject trap)
	{
		hasTrap = true;
		this.trap = Instantiate(trap, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity, transform) as GameObject;
	}

	public void AttachScrap(GameObject scrapHeap)
	{
		HasScrap = true;
		this.scrapHeap = Instantiate(scrapHeap, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity, transform) as GameObject;
		this.scrapHeap.transform.localRotation = Quaternion.Euler(0, Random.Range(0, 4) * 90f, 0);

	}

	public void AttachTower(GameObject tower)
	{
		this.tower = Instantiate(tower, scrapHeap.transform.position + new Vector3(0, 0.4f, 0), Quaternion.identity, transform) as GameObject;
		hasTower = true;
	}

	public GameObject Tower
	{
		get { return tower; }
	}
}