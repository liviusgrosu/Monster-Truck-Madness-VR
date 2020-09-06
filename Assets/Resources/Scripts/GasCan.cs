using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasCan : MonoBehaviour {

    public int gasPoints;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.parent.gameObject.GetComponent<RCCar>().AddFuel(gasPoints);
            Destroy(gameObject);
        }
    }
}
