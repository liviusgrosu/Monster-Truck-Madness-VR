using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AxleInfo {
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor; // is this wheel attached to motor?
    public bool steering; // does this wheel apply steer angle?
}

public class RCCar : MonoBehaviour {

    public Camera carCam;
    public GameObject dashboard;
    public Transform spawn;
    public List<AxleInfo> axleInfos; // the information about each individual axle
    public float maxMotorTorque; // maximum torque the motor can apply to wheel
    public float maxSteeringAngle; // maximum steer angle the wheel can have

    private float currFuel, maxFuel = 100f;
    private int gearVal;
    private float steeringVal;
    private float gasVal, brakeVal;
    private RCCarRoot root;
    private Quaternion startingRot;
    public Selectable select;

    void Start()
    {
        startingRot = transform.rotation;
        currFuel = maxFuel;
    }

    public void FixedUpdate()
    {

        if (select.isHighlighted)
        {
            float brake = maxMotorTorque * brakeVal;
            float motor = maxMotorTorque * gasVal;
            float steering = maxSteeringAngle * steeringVal;

            dashboard.GetComponent<Dashboard>().getSpeed(GetComponent<Rigidbody>().velocity.magnitude);
            dashboard.GetComponent<Dashboard>().getGearState(gearVal);
            switch (gearVal)
            {
                case 0:
                    motor = 0f;
                    break;
                case 1:
                    motor = maxMotorTorque * gasVal;
                    break;
                case 2:
                    motor = maxMotorTorque * gasVal * -1f;
                    break;
            }

            foreach (AxleInfo axleInfo in axleInfos)
            {
                if (axleInfo.steering)
                {
                    axleInfo.leftWheel.steerAngle = steering;
                    axleInfo.rightWheel.steerAngle = steering;
                }
                if (axleInfo.motor)
                {
                    axleInfo.leftWheel.motorTorque = motor;
                    axleInfo.rightWheel.motorTorque = motor;

                    axleInfo.leftWheel.brakeTorque = brake;
                    axleInfo.rightWheel.brakeTorque = brake;
                }
            }

            if (currFuel > 0.0f)
            {
                currFuel -= Time.deltaTime;
            }

            else
            {
                Destroy(this.gameObject);
            }
        }
    }

	public void getSteeringInfo(float value)
	{
        steeringVal = value;
    }

	public void getGasInfo(float value)
	{
        gasVal = value;
    }

	public void getBrakeInfo(float value)
	{
        brakeVal = value;
	}

	public void getGearInfo(int value)
	{
        gearVal = value;
    }

    public void AddFuel(float val)
    {
        currFuel += val;
        if (currFuel > maxFuel) currFuel = maxFuel;
    }

    public float GetCurrFuel()
    {
        return currFuel;
    }

    public float GetMaxFuel()
    {
        return maxFuel;
    }


    public Camera getCam()
    {
        return carCam;
    }

    public void Respawn()
    {
        transform.position = spawn.position;
        transform.rotation = startingRot;

        gasVal = 0;
        brakeVal = 0;

        GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
        GetComponent<Rigidbody>().angularVelocity = new Vector3(0f, 0f, 0f);
    }
}
    

