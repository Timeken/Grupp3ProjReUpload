using UnityEngine;
using System.Collections;

public class CombatTower : TowerScript
{
	public float speed;
	public float sideRange;
	public float timeBetweenShots;
	public float reloadTimer;
	public int maxAmmo;
	public int maxMags;
	public int mags;
	public int damage;
	public bool canFire = true;
	public GameObject shot;
	public bool targetFirst = false;

	protected Animator anim;
	protected float diff;
	public int ammo;
	protected int magnets;
	protected Quaternion defaultRotation;
	protected Quaternion targetRotation;
	protected GameObject target;
	protected GameObject bulletSpawn;
	protected GameObject rotatingPart;
	protected EnemyManager enemyManager;

	protected float shotTimer;

	protected GameObject thisShot;

	protected void Start()
	{
		anim = GetComponent<Animator>();

		enemyManager = GameObject.Find("GameManager").GetComponent<EnemyManager>();

		ammo = maxAmmo;
		mags = maxMags;

		defaultRotation = transform.rotation;

		foreach (Transform t in GetComponentsInChildren<Transform>())
		{
			switch (t.gameObject.name)
			{
				case "BulletSpawn":
					bulletSpawn = t.gameObject;
					break;

				case "Dome":
					rotatingPart = t.gameObject;
					break;

				default:
					break;
			}
		}
	}

	protected void Update()
	{
		if (target != null)
		{
			diff = (target.transform.position - transform.position).sqrMagnitude;

			if (diff > range || target.GetComponent<Enemy>().GetDead())
			{
				FindClosestEnemy();
			}

			targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
			targetRotation.z = 0;
			targetRotation.x = 0;

			rotatingPart.transform.rotation = Quaternion.RotateTowards(rotatingPart.transform.rotation, targetRotation, speed * Time.deltaTime);

			if (canFire)
			{
				if (ammo > 0 && target != null && rotatingPart.transform.rotation.y >= target.transform.rotation.y - sideRange
					&& rotatingPart.transform.rotation.y <= target.transform.rotation.y + sideRange
					&& Time.time >= shotTimer + timeBetweenShots && !target.GetComponent<Enemy>().GetDead() && target.GetComponent<Enemy>().targetable)
				{
					Fire();
				}

				else if (ammo <= 0)
				{
					StartCoroutine("Reload");
				}
			}
		}

		else if (target == null)
		{
			FindClosestEnemy();
		}
	}

	protected void FindClosestEnemy()
	{
		if (enemyManager.Enemies.Count > 0)
		{
			float closestDistance = range;

			foreach (GameObject go in enemyManager.Enemies)
			{
				if (go)
				{
					diff = (go.transform.position - transform.position).sqrMagnitude;

					if (diff <= range && diff <= closestDistance && !go.GetComponent<Enemy>().GetDead())
					{
						closestDistance = diff;
						target = go;
					}
				}
			}
		}
	}

	protected virtual void Fire()
	{
		GetComponent<AudioSource>().Play();
		ammo--;
		if (anim)
		{
			anim.SetTrigger("fire");
		}
	}

	IEnumerator Reload()
	{
		canFire = false;

		yield return new WaitForSeconds(reloadTimer);

		if (mags <= maxMags)
		{
			ammo = maxAmmo;
			canFire = true;
			mags--;
		}

		if (mags <= 0 && ammo <= 0)
		{
			canFire = false;
		}
	}

	public void AmmoRestock()
	{
		mags = maxMags;
		ammo = maxAmmo;
		canFire = true;
	}
}