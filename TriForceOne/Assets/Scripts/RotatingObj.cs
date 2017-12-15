using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingObj : MonoBehaviour {

    private Transform from;
    private bool rotating;
    private float speed = 1;
    private Quaternion fianlRotation;

    public void Rotation()
    {
        from = gameObject.transform;
        fianlRotation = Quaternion.Euler(0, from.transform.rotation.y + 90, 0);
        rotating = !rotating;
    }

    private void Update()
    {
        if (rotating)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, fianlRotation, Time.deltaTime * speed);
        }
    }
}
