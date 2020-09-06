using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour {

    public Material nonSelectedMat, selectedMat;
    public bool isHighlighted = false;

    private RCCarRoot root;

	// Use this for initialization
	void Start () {
        root = transform.parent.transform.parent.GetComponent<RCCarRoot>();               
        DeselectObject();
    }
    
    public void SelectObject() {
        GetComponent<Renderer>().material = selectedMat;
    }

    public void DeselectObject() {
        GetComponent<Renderer>().material = nonSelectedMat;       
    }

    public void PermaSelectObject()
    {
        root.setCurrentCar(gameObject.transform.parent.gameObject);        
    }

    public void PermaDeselectObject()
    {
        root.setCurrentCar(null);      
    }
}
