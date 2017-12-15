using UnityEngine;
using System.Collections;

public class EntranceScript : MonoBehaviour
{
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Enemy") && other.gameObject.GetComponent<Enemy>())
		{
			other.gameObject.GetComponent<Enemy>().ToggleTargetable();
		}
	}
}
