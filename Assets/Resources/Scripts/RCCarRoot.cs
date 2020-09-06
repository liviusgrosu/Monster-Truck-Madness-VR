using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RCCarRoot : MonoBehaviour {

	public List<GameObject> cars;
	private GameObject currCar;

	// Use this for initialization
	void Start () {
		currCar = cars[0];
		currCar.GetComponent<RCCar>().getCam().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setCurrentCar(GameObject car)
	{
		if(car != null) car.GetComponent<RCCar>().getCam().enabled = true;
		else currCar.GetComponent<RCCar>().getCam().enabled = false;
		currCar = car;
	}

	public GameObject getCurrentCar()
	{
		return currCar;
	}

	public bool hasCurrentCar()
	{
		if(currCar != null) return true;
		else return false;
	}
}
