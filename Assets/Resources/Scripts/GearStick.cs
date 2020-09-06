using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearStick : MonoBehaviour, ITool {

    private bool isInteractingWith;
	private Vector3 newRelativeHandPosition, relativeHandPosition;
	public GameObject constraint1, constraint2;

	public GameObject car;
	
	private float distToConst1, distToConst2, distToController;

	public bool gearX, gearY, gearZ;

	//0 - Neutral
	//1 - Forward
	//2 - Reverse
	private int drivingState;
	private bool inGear = true;

	private Vector3 movingVec = Vector3.zero;
	
	void Start()
	{
		if(gearX) movingVec.x = 0.11f;
		if(gearY) movingVec.y = 0.11f;
		if(gearZ) movingVec.z = 0.11f;		
	}

	// Update is called once per frame
	void Update () {
        if (isInteractingWith && inGear) {
			if(gearX) distToController = transform.position.x - newRelativeHandPosition.x;
			if(gearY) distToController = transform.position.y - newRelativeHandPosition.y;
			if(gearZ) distToController = transform.position.z - newRelativeHandPosition.z;	
            
        }
		if(car != null && car.GetComponent<RCCarRoot>().hasCurrentCar())
        {
        	car.GetComponent<RCCarRoot>().getCurrentCar().GetComponent<RCCar>().getGearInfo(drivingState);
		}
	}

	public void Interact()
	{
		Vector3 outOfGearDir = transform.position;
		Vector3 distVec = transform.position;
		

		if(gearX) 
		{
			outOfGearDir.x = newRelativeHandPosition.x;
			distToConst1 = transform.position.x - constraint1.transform.position.x;
			distToConst2 = transform.position.x - constraint2.transform.position.x;
		}
		if(gearY) 
		{
			outOfGearDir.y = newRelativeHandPosition.y;
			distToConst1 = transform.position.y - constraint1.transform.position.y;
			distToConst2 = transform.position.y - constraint2.transform.position.y;
		}
		if(gearZ) 
		{
			outOfGearDir.z = newRelativeHandPosition.z;
			distToConst1 = transform.position.z - constraint1.transform.position.z;
			distToConst2 = transform.position.z - constraint2.transform.position.z;
		}

		if(distToConst1 < 0.0f && distToConst2 > 0.0f)
		{
			if(drivingState != 0)
			{
				inGear = false;
				drivingState = 0;
			}
			transform.position = outOfGearDir;
		}
		else if(distToConst1 >= 0.0f) 
		{
			if(drivingState == 0)
			{
				inGear = true;
				drivingState = 1;
				
			}
			else if(drivingState != 0 && distToController > 0.1f) transform.position -= movingVec;
		}
		else if(distToConst2 <= 0.0f) 
		{
			if(drivingState == 0)
			{
				inGear = true;
				drivingState = 2;
			}
			else if(drivingState != 0 && distToController < -0.1f) transform.position +=  movingVec;
			
		}
	}
    public void GetNewPos(Vector3 pos)
	{
		newRelativeHandPosition = pos;
	}
    public void GetCurrPos(Vector3 pos)
	{
		relativeHandPosition = pos;
	}
    public void BeingInteractedWith(bool state){
		isInteractingWith = state;
	}
}
