using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingObj : MonoBehaviour {

    private Transform from;
    private bool rotating;
    private float speed = 1;
    private float rotAmount = 0;
    private Quaternion fianlRotation;

    private void Start()
    {
        from = gameObject.transform;
    }

    public void Rotation()
    {
        //from = gameObject.transform;
        rotAmount += 90;       
        print(rotAmount);
        transform.Rotate(0, rotAmount, 0);
        //fianlRotation = Quaternion.Euler(0,rotAmount, 0);
        rotating = !rotating;
    }

    private void Update()
    {
        if (rotating)
        {
            //transform.Rotate(0, rotAmount, 0);
            //transform.rotation = Quaternion.Lerp(transform.rotation, fianlRotation, Time.deltaTime * speed);
        }
    }
}
