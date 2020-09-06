using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedal : MonoBehaviour{

    private bool init;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void Interact() {
        if(!init) {
            init = true;
        }
    }

    public void getNewPos(Vector3 pos) {

    }

    public void getCurrPos(Vector3 pos) {

    }
}
