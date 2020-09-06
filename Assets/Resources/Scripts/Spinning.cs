using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinning : MonoBehaviour {

    [Header("Spinning")]
    public float turnSpeed;
    public bool x, y, z;
    
    [Header("Moving")]
    public float moveSpeed;
    public float moveDist;

    private Vector3 point1, point2;
    private bool toPoint1;

    private void Start()
    {
        toPoint1 = true;

        point1 = transform.position + new Vector3(0, moveDist, 0);
        point2 = transform.position - new Vector3(0, moveDist, 0);
    }

    private void Update() {
        //Spin the object depending on what axis the user requested in edit mode
        if (x) transform.Rotate(turnSpeed, 0, 0);
        if (y) transform.Rotate(0, turnSpeed, 0);
        if (z) transform.Rotate(0, 0, turnSpeed);

        if(toPoint1)
        {
            transform.position = Vector3.Lerp(transform.position, point1, moveSpeed);
            if (Vector3.Distance(transform.position, point1) < 0.01f)
                toPoint1 = false;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, point2, moveSpeed);
            if (Vector3.Distance(transform.position, point2) < 0.01f)
                toPoint1 = true;
        }
    }
}
