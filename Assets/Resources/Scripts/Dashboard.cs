using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dashboard : MonoBehaviour {

    private GameObject currCar;
	public Text speedometerText, gearIndicatorText, scoreText;
    public Material fuelMat;
    private int currScore, maxScore;

    private void Start()
    {
        fuelMat.SetFloat("_Cutoff", 0f);
        currScore = 0;

        GameObject[] scoreObj = GameObject.FindGameObjectsWithTag("PointObj");
        foreach (GameObject objs in scoreObj)
        {
            maxScore += objs.GetComponent<ScoreObject>().scorePoints;
        }
    }

    private void Update()
    {
        if (currScore == maxScore) { }
            //print("you win");

        if(currCar != null)
        {
            float fuelPerc = 1.0f - (currCar.GetComponent<RCCar>().GetCurrFuel() / currCar.GetComponent<RCCar>().GetMaxFuel());
            //print(currCar.GetComponent<RCCar>().GetCurrFuel() / currCar.GetComponent<RCCar>().GetMaxFuel());
            fuelMat.SetFloat("_Cutoff", fuelPerc);
        }
    }

    public void AddScore(int score)
    {
        currScore += score;
        scoreText.text = currScore.ToString();
        if(currScore < 10) scoreText.text = "0" + scoreText.text;        
    }

    public void RemoveScore(int score)
    {
        currScore -= score;
        scoreText.text = currScore.ToString();
        if(currScore < 10) scoreText.text = "0" + scoreText.text;
    }

    public void getGearState(int state)
	{
		if(state == 0) gearIndicatorText.text = "N";
		if(state == 1) gearIndicatorText.text = "F";
		if(state == 2) gearIndicatorText.text = "R";
	}

	public void getSpeed(float speed)
	{
		speedometerText.text = (int)speed + "\n\n km/h";
	}

    public void GetCurrCar(GameObject car)
    {
        currCar = car;
    }
}
