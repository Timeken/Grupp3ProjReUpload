using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class BuildingManagerScript : MonoBehaviour
{
	public bool buildScrap = false;

	public GameObject start;
	public GameObject target;
	public GameObject bulletTower;
	public GameObject magnetTower;
	public GameObject lazerTower;
	public GameObject missileTower;
	GameObject gameManager;
	protected GameObject lastClickedFoundation;
	public GameObject[] scrapHeaps;

	int scrapIndex = 0;
	int lastScrapIndex = 0;
	public int scrapHeapCost;

	public ScrapManager scrapManager;
	public LayerMask layerMask;
	protected FoundationScript clickedFoundationScript;

	RaycastHit hit;
	Ray ray;
	NavMeshPath path;

	// Use this for initialization
	void Start()
	{
		//Ger variables dess värde, skapar rätt referenser
		path = new UnityEngine.AI.NavMeshPath();
		start = GameObject.Find("SpawnPoint");
		target = GameObject.Find("FinishPoint");
		gameManager = GameObject.Find("GameManager");
		scrapManager = gameManager.GetComponent<ScrapManager>();

		NavMesh.CalculatePath(start.transform.position, target.transform.position, NavMesh.AllAreas, path);
	}

	// Update is called once per frame
	void Update()
	{
		//klickas vänster musknapp ned när spelet inte är pausat eller över...
		if (Input.GetMouseButtonDown(0) && !gameManager.GetComponent<MenuManagerScript>().GetPaused() && !gameManager.GetComponent<GameOverManager>().GameOver && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(-1))
		{
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			//...körs en raycast. Som ignorerar vissa lager och träffar resten
			if (Physics.Raycast(ray, out hit, 1000, layerMask))
			{
				if (hit.transform.tag.Equals("Foundation"))
				{
					ClickedOnFoundation(hit.transform);
					if (!clickedFoundationScript.clicked && !clickedFoundationScript.HasScrap)
					{
						//Ifall grunden ej är byggd på samt ej klickad på innan körs en koll för att se ifall platsen får byggas på
						StartCoroutine("CheckPath");
					}

					else
					{
						clickedFoundationScript.ClickedOn(true);
					}
				}

				else if (hit.transform.CompareTag("ScrapHeap"))
				{
					ClickedOnFoundation(hit.transform.parent);
					clickedFoundationScript.ClickedOn(true);
				}

				else if (hit.transform.CompareTag("Tower"))
				{
					if (!hit.transform.gameObject.GetComponent<Magnet>())
					{
						ClickedOnFoundation(hit.transform.GetComponent<TowerScript>().Foundation.transform);
						clickedFoundationScript.ClickedOn(true);
					}
					else if (hit.transform.gameObject.GetComponent<Magnet>())
					{
						ClickedOnFoundation(hit.transform.parent);
						clickedFoundationScript.ClickedOn(true);
					}
				}

				else if (hit.transform.CompareTag("GroundTrap"))
				{
					ClickedOnFoundation(hit.transform.parent);
					clickedFoundationScript.ClickedOn(true);
				}
				else if (hit.transform.CompareTag("BugFix"))
				{
					ClickedOnFoundation(hit.transform.parent.parent);
					clickedFoundationScript.ClickedOn(true);
				}
			}
		}

		if (lastClickedFoundation)
		{
			if (clickedFoundationScript.HasScrap && !clickedFoundationScript.HasTower && Input.GetKeyDown(KeyCode.T))
			{
				BuildTower(bulletTower);
			}

			else if (clickedFoundationScript.HasScrap && !clickedFoundationScript.HasTower && Input.GetKeyDown(KeyCode.M))
			{
				BuildTower(magnetTower);
			}

			else if (clickedFoundationScript.HasScrap && !clickedFoundationScript.HasTower && Input.GetKeyDown(KeyCode.L))
			{
				BuildTower(lazerTower);
			}

			else if (clickedFoundationScript.HasScrap && !clickedFoundationScript.HasTower && Input.GetKeyDown(KeyCode.B))
			{
				BuildTower(missileTower);
			}

			else if (clickedFoundationScript.valid && !clickedFoundationScript.HasScrap && Input.GetKeyDown(KeyCode.E))
			{
				BuildScrap();
			}

			else if (clickedFoundationScript.HasTower && Input.GetKeyDown(KeyCode.Alpha1))
			{
				ValidateTowerUpgrade(0);
			}

			else if (clickedFoundationScript.HasTower && Input.GetKeyDown(KeyCode.Alpha2))
			{
				ValidateTowerUpgrade(1);
			}

			else if (clickedFoundationScript.HasTower && Input.GetKeyDown(KeyCode.Alpha3))
			{
				ValidateTowerUpgrade(2);
			}

			else if (clickedFoundationScript.HasScrap && Input.GetKeyDown(KeyCode.Q))
			{
				Sell();
			}

			else if (clickedFoundationScript && Input.GetKeyDown(KeyCode.LeftArrow))
			{
				ray = new Ray(lastClickedFoundation.transform.position, new Vector3(-1, 0, 0));
				if (Physics.Raycast(ray, out hit, 2f, layerMask))
				{
					if (hit.transform.CompareTag("Foundation"))
					{
						ClickedOnFoundation(hit.transform);

						if (!clickedFoundationScript.HasScrap)
						{
							StartCoroutine("CheckPath");
						}
						else
						{
							clickedFoundationScript.ClickedOn(true);
						}
					}
				}
			}

			else if (clickedFoundationScript && Input.GetKeyDown(KeyCode.RightArrow))
			{
				ray = new Ray(lastClickedFoundation.transform.position, new Vector3(1, 0, 0));
				if (Physics.Raycast(ray, out hit, 2f, layerMask))
				{
					if (hit.transform.CompareTag("Foundation"))
					{
						ClickedOnFoundation(hit.transform);

						if (!clickedFoundationScript.HasScrap)
						{
							StartCoroutine("CheckPath");
						}
						else
						{
							clickedFoundationScript.ClickedOn(true);
						}
					}
				}
			}

			else if (clickedFoundationScript && Input.GetKeyDown(KeyCode.UpArrow))
			{
				ray = new Ray(lastClickedFoundation.transform.position, new Vector3(0, 0, 1));
				if (Physics.Raycast(ray, out hit, 2f, layerMask))
				{
					if (hit.transform.CompareTag("Foundation"))
					{
						ClickedOnFoundation(hit.transform);

						if (!clickedFoundationScript.HasScrap)
						{
							StartCoroutine("CheckPath");
						}
						else
						{
							clickedFoundationScript.ClickedOn(true);
						}
					}
				}
			}

			else if (clickedFoundationScript && Input.GetKeyDown(KeyCode.DownArrow))
			{
				ray = new Ray(lastClickedFoundation.transform.position, new Vector3(0, 0, -1));
				if (Physics.Raycast(ray, out hit, 2f, layerMask))
				{
					if (hit.transform.CompareTag("Foundation"))
					{
						ClickedOnFoundation(hit.transform);

						if (!clickedFoundationScript.HasScrap)
						{
							StartCoroutine("CheckPath");
						}
						else
						{
							clickedFoundationScript.ClickedOn(true);
						}
					}
				}
			}
		}
	}

	public void Reset()
	{
		lastClickedFoundation = null;
	}

	public void Sell()
	{
		if (clickedFoundationScript.HasTower)
		{
			clickedFoundationScript.SellTower();
		}
		else
		{
			clickedFoundationScript.SellScrap();
		}
	}

	public void BuildScrap()
	{
		if (scrapManager.ScrapChange(-scrapHeapCost))
		{
			while (scrapIndex == lastScrapIndex)
			{
				scrapIndex = Random.Range(0, 4);
			}

			lastScrapIndex = scrapIndex;
			clickedFoundationScript.AttachScrap(scrapHeaps[scrapIndex]);
			clickedFoundationScript.ClickedOn(true);
		}
	}

	public void BuildTower(GameObject tower)
	{
		if (scrapManager.ScrapChange(-tower.GetComponent<TowerScript>().scrapCost))
		{
			GameObject g = tower;
			g.GetComponent<TowerScript>().Foundation = lastClickedFoundation;
			clickedFoundationScript.AttachTower(g);
			clickedFoundationScript.ClickedOn(true);
		}
	}

	IEnumerator CheckPath()
	{
		lastClickedFoundation.GetComponent<UnityEngine.AI.NavMeshObstacle>().enabled = true;
		//Tas denna paus bort fungerar inte checken ¯\_(ツ)_/¯
		yield return new WaitForSeconds(0);
		StartCoroutine("CalculatePath");
		lastClickedFoundation.GetComponent<UnityEngine.AI.NavMeshObstacle>().enabled = false;

		//Kollar avstånd mellan sista punken i pathen, ifall denna är nära finishpoint så räknas detta som en godkänd path
		Vector3 compVec = new Vector3(path.corners[path.corners.Length - 1].x, path.corners[path.corners.Length - 1].y, path.corners[path.corners.Length - 1].z);
		if (Mathf.Abs(compVec.x - target.transform.position.x) < 0.5f)
		{
			if (Mathf.Abs(compVec.z - target.transform.position.z) < 0.5f)
			{
				if (buildScrap) { BuildScrap(); }
				clickedFoundationScript.valid = true;
				clickedFoundationScript.ClickedOn(true);
			}
			else
			{
				GameObject.Find("InvalidBuildSound").GetComponent<AudioSource>().Play();
				clickedFoundationScript.valid = false;
				clickedFoundationScript.ClickedOn(true);
			}
		}
		else
		{
			GameObject.Find("InvalidBuildSound").GetComponent<AudioSource>().Play();
			clickedFoundationScript.valid = false;
			clickedFoundationScript.ClickedOn(true);
		}
	}

	//Räknar ut den faktiska vägen för kollen
	IEnumerator CalculatePath()
	{
		UnityEngine.AI.NavMesh.CalculatePath(start.transform.position, target.transform.position, UnityEngine.AI.NavMesh.AllAreas, path);
		yield return null;
	}

	void ClickedOnFoundation(Transform foundation)
	{
		if (lastClickedFoundation)
		{
			lastClickedFoundation.GetComponent<FoundationScript>().Declick();
		}

		lastClickedFoundation = foundation.gameObject;
		clickedFoundationScript = lastClickedFoundation.GetComponent<FoundationScript>();
	}

	public void ValidateTowerUpgrade(int upgradeIndex)
	{
		if (clickedFoundationScript.Tower.GetComponent<TowerScript>().AvailableUpgrade(upgradeIndex))
		{
			if (scrapManager.ScrapChange(-clickedFoundationScript.tower.GetComponent<TowerScript>().GetUpgradeCost(upgradeIndex)))
			{
				clickedFoundationScript.tower.BroadcastMessage("Upgrade", upgradeIndex);
				clickedFoundationScript.ClickedOn(true);
			}
		}
	}
}
