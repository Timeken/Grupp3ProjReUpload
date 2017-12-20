using UnityEngine;
using System.Collections;

public class MainMenuMurderer : MonoBehaviour {

	void OnTriggerEnter(Collider other)
	{
		Destroy(other.gameObject);
	}
}
