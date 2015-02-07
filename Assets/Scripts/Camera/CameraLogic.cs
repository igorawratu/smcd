using UnityEngine;
using System.Collections;

public class CameraLogic : MonoBehaviour 
{
    public Vector3 moveVel = new Vector3(0.0f, 0.0f);
    public static CameraLogic camLogic;

    void Awake()
    {
        camLogic = this;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        moveVel.x = VariableSpeed.current;
        Vector3 currentPos =  gameObject.transform.position;
        currentPos += moveVel * Time.deltaTime;
        currentPos.z = -10;

        gameObject.transform.position = currentPos;



	}
}
