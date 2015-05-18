using UnityEngine;
using System.Collections;

public class ParticleMovement : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}


    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 moveVel = Vector3.zero;
        moveVel.x = VariableSpeed.current;
        Vector3 currentPos = gameObject.transform.position;
        currentPos += moveVel * Time.deltaTime;
        //currentPos.z = -10;

        gameObject.transform.position = currentPos;
    }
}
