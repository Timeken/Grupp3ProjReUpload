using UnityEngine;
using System.Collections;

public class Shot : MonoBehaviour
{
    public float speed;
    public int damage;
    public GameObject target;
	public float blastRadius;

	void Update()
    {
        if (target == null || target.GetComponent<Enemy>().GetDead())
        {
            Destroy(gameObject);
        }
        else
        {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
        }
    }

    public void SetTarget(GameObject go)
    {
        this.target = go;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().TakeDamage(damage);
            Destroy(this.gameObject);
        }
    }
}
