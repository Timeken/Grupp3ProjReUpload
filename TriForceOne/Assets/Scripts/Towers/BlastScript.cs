using UnityEngine;
using System.Collections;

public class BlastScript : MonoBehaviour
{
    public void ExplosionDamage(Vector3 center, float radius, int damage)  //Hittar & skadar alla fiender inom en viss range
    {
        damage = gameObject.GetComponentInParent<ExplosiveShot>().damage;

        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        foreach (Collider c in hitColliders)
        {
            if (c.gameObject.CompareTag("Enemy"))
            {
                c.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            }
        }
    }
}