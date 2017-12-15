using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrapScript : MonoBehaviour
{
	public int damage;
	public int reloadTime;
	public bool canAttack = true;
	public List<GameObject> enemies = new List<GameObject>();
    
	void OnTriggerEnter(Collider col)
	{
		if (col.CompareTag("Enemy"))
			enemies.Add(col.gameObject);
	}

	void OnTriggerStay(Collider col)
	{
		if (canAttack && col.gameObject != null && col.gameObject.CompareTag("Enemy") && col.gameObject.GetComponent<Enemy>() != null && !col.gameObject.GetComponent<Enemy>().GetDead())
		{
			foreach (GameObject g in enemies)
			{
				g.GetComponent<Enemy>().TakeDamage(damage);
			}
			GetComponent<BoxCollider>().enabled = false;
			enemies.Clear();
			GetComponent<Animator>().SetTrigger("TrapTrigger");
			canAttack = false;
			StartCoroutine("Reload");
		}
	}

	IEnumerator Reload()
	{
		yield return new WaitForSeconds(reloadTime);
		canAttack = true;
		GetComponent<BoxCollider>().enabled = true;
	}
}