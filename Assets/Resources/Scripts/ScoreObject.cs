using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreObject : MonoBehaviour {
    public int scorePoints;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            GameObject.Find("Canvas").GetComponent<Dashboard>().AddScore(scorePoints);
            Destroy(gameObject);
        }
    }
}
