using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingObj : MonoBehaviour {

    private Transform from;
    private bool rotating;
    private float speed = 5;
    private float rotAmount = 0;
    private Quaternion fianlRotation;

    private void Start()
    {
        from = gameObject.transform;
    }

    public void Rotation()
    {
        rotAmount += 90;       
        print(rotAmount);
        if (rotAmount == 90)
        {
            fianlRotation = Quaternion.Euler(0, 90, 0);
            print("turn 1");
        }
        else if (rotAmount == 180)
        {
            fianlRotation = Quaternion.Euler(0, 0, 0);
            transform.Rotate(0, 90, 0);
            print("turn 2");
        }
        else if (rotAmount == 270)
        {
            fianlRotation = Quaternion.Euler(0, -90, 0);
            print("turn 3");
        }
        else if (rotAmount == 360)
        {
            fianlRotation = Quaternion.Euler(0, 0, 0);
            transform.Rotate(0, 90, 0);
            print("turn 4");
            rotAmount = 0;
        }

        rotating = !rotating;
    }

    private void Update()
    {
        if (rotating)
        {
            transform.rotation = Quaternion.Lerp(fianlRotation, fianlRotation, Time.deltaTime * speed);
        }
    }
}
