using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringWheel : MonoBehaviour, ITool {

    public GameObject hand;
    public GameObject car;

    private Vector3 newRelativeHandPosition, relativeHandPosition;

    public bool wheel, pedal;
    public bool gasPedal, brakePedal;
    public bool handX, handY, handZ;

    private int minDeg, maxDeg;

    private float curAng;
    private bool isInteractingWith;

    private Quaternion origRot;
    public float retractSpeed;
    public bool canRetract;

    private float angle;

    private void Start() {
        curAng = 0.0f;

        origRot = transform.rotation;

        if(wheel) {
            minDeg = -900;
            maxDeg = 900;
        }

        else if(pedal) {
            minDeg = 0;
            maxDeg = 90;
        }
    }

    private void Update() {
        if(!isInteractingWith && canRetract && pedal) {
            curAng = Quaternion.Angle(transform.rotation, origRot);
            transform.rotation = Quaternion.Slerp(transform.rotation, origRot, Time.deltaTime * retractSpeed);
        }

        if(car.GetComponent<RCCarRoot>().hasCurrentCar())
        {
            if(wheel)
                car.GetComponent<RCCarRoot>().getCurrentCar().GetComponent<RCCar>().getSteeringInfo(curAng / maxDeg * -1);
            if(pedal) {
                if (gasPedal) car.GetComponent<RCCarRoot>().getCurrentCar().GetComponent<RCCar>().getGasInfo(curAng / maxDeg);
                if (brakePedal) car.GetComponent<RCCarRoot>().getCurrentCar().GetComponent<RCCar>().getBrakeInfo(curAng / maxDeg);
            }
        }
    }

    public void Interact() {

        Vector3 angleNew = Vector3.zero;
        Vector3 angleCurr = Vector3.zero;

        if (handX) {
            angleNew = new Vector3(0f, newRelativeHandPosition.y - transform.position.y, newRelativeHandPosition.z - transform.position.z);
            angleCurr = new Vector3(0f, relativeHandPosition.y - transform.position.y, relativeHandPosition.z - transform.position.z);
        }

        if (handZ) {
            angleNew = new Vector3(newRelativeHandPosition.x - transform.position.x, newRelativeHandPosition.y - transform.position.y, 0f);
            angleCurr = new Vector3(relativeHandPosition.x - transform.position.x, relativeHandPosition.y - transform.position.y, 0f);
        }

        angle = Vector3.SignedAngle(angleNew, angleCurr, transform.forward);

        if (pedal) curAng = Quaternion.Angle(transform.rotation, origRot);
        else curAng += angle;
        
        Mathf.Clamp(curAng, minDeg, maxDeg);


        if (curAng <= maxDeg && curAng >= minDeg ) {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + angle);
        }
    }

    public void GetNewPos(Vector3 pos) {
        newRelativeHandPosition = pos;
    }
    public void GetCurrPos(Vector3 pos) {
        relativeHandPosition = pos;
    }

    public void BeingInteractedWith(bool state) {
        isInteractingWith = state;
    }
}
