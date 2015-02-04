﻿using UnityEngine;
using System.Collections;

public class VariableSpeed : MonoBehaviour {

    public Vector2 speedMinMax = new Vector2(8.0f, 20.0f);
    public Vector2 speedsBoostMinMax = new Vector2(2.0f, 5.0f);
    public Vector2 cameraSizeMinMax = new Vector2(5.0f, 6.0f);
    public static float current = 0.0f;
    public static float currentBoost = 0.0f;
    public float speedBoostTime = 0.0f;
    public static float currentSpeedBoostTime = 0.0f;
    public float timeDelay = 20.0f;
    float timeElapsed = 0.0f;


    public Vector2 speedCloudVariable = new Vector2(0.0f, 0.0f);
    public static float currentCloudSpeed = 0.0f;

	// Use this for initialization
	void Start () 
    {
        current = speedMinMax.x;
        currentBoost = speedsBoostMinMax.x;
        currentSpeedBoostTime = speedBoostTime;
        currentCloudSpeed = speedCloudVariable.x;

        StartCoroutine(levelSpeedController());
	}

    private IEnumerator levelSpeedController()
    {
        bool canUpdate = true;

        while (canUpdate)
        {
            timeElapsed += Time.deltaTime;

            if (timeElapsed < timeDelay)
            {
                current = Mathf.Lerp(speedMinMax.x, speedMinMax.y, timeElapsed / timeDelay);
                currentBoost = Mathf.Lerp(speedsBoostMinMax.x, speedsBoostMinMax.y, timeElapsed / timeDelay);
                currentCloudSpeed = Mathf.Lerp(speedCloudVariable.x, speedCloudVariable.y, timeElapsed / timeDelay);
                Camera.main.orthographicSize = Mathf.Lerp(cameraSizeMinMax.x, cameraSizeMinMax.y, timeElapsed / timeDelay);
                yield return null;
            }
            else
            {
                current = speedMinMax.y;
                currentBoost = speedsBoostMinMax.y;
                currentCloudSpeed = speedCloudVariable.y;
                Camera.main.orthographicSize = cameraSizeMinMax.y;
                canUpdate = false;
            }
        }
    }

    // Update is called once per frame
    //void Update()
    //{
    //    timeElapsed += Time.deltaTime;
    //    if(timeElapsed<timeDelay)
    //    {
    //        current = Mathf.Lerp(speedMinMax.x, speedMinMax.y, timeElapsed / timeDelay);
    //        currentBoost = Mathf.Lerp(speedsBoostMinMax.x, speedsBoostMinMax.y, timeElapsed / timeDelay);
    //        currentCloudSpeed = Mathf.Lerp(speedCloudVariable.x, speedCloudVariable.y, timeElapsed / timeDelay);
    //    }
    //    else
    //    {
    //        current = speedMinMax.y;
    //        currentBoost = speedsBoostMinMax.y;
    //        currentCloudSpeed = speedCloudVariable.y;
    //    }
    //}
}
