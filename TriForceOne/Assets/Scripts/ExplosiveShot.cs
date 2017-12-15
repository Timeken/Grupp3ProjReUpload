using UnityEngine;
using System.Collections;

public class ExplosiveShot : Shot
{
    SphereCollider blastCollider;

    protected override void OnTriggerEnter(Collider other)
    {
        blastCollider = GetComponentInChildren<SphereCollider>();
        if (other.CompareTag("Enemy"))
        {
            blastCollider.enabled = true;
            GetComponentInChildren<BlastScript>().ExplosionDamage(this.transform.position, blastRadius, this.damage);
            Destroy(this.gameObject);
        }
    }
}