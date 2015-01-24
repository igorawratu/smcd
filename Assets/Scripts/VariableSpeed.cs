using UnityEngine;
using System.Collections;

public class VariableSpeed : MonoBehaviour {

    public Vector2 speedsVariables = new Vector2(0.0f,0.0f);
    public static float current = 0.0f;
    public float timeDelay = 20.0f;
    float timeElapsed = 0.0f;

	// Use this for initialization
	void Start () 
    {
        current = speedsVariables.x;
	}

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
        if(timeElapsed<timeDelay)
        {
            current = Mathf.Lerp(speedsVariables.x, speedsVariables.y, timeElapsed/timeDelay);
        }
        else
        {
            current = speedsVariables.y;
        }
    }
}
