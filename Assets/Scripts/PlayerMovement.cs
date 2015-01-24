using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour 
{
    Vector3 moveVel = Vector3.zero;
    public Vector3 rightAcceleration = Vector3.zero;
    public Vector3 maxVel = Vector3.zero;
    public float jumpVel = 0.0f;
	// Use this for initialization
	void Start () 
    {
        moveVel = CameraLogic.camLogic.moveVel;
        Vector3 vel = gameObject.rigidbody2D.velocity;
        vel.x += 10;
        gameObject.rigidbody2D.velocity = vel;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        //Vector3 pos = gameObject.rigidbody2D.position;
        //pos.x += moveVel.x;
        //gameObject.transform.position = pos;

        Vector3 vel = gameObject.rigidbody2D.velocity;
        vel += rightAcceleration*Time.deltaTime;


        if (Input.GetKey(KeyCode.UpArrow))
        {
            //Vector3 vel = gameObject.rigidbody2D.velocity;
            vel.y += jumpVel* Time.deltaTime;
        }

        if (vel.x >= maxVel.x)
        {
            vel.x = maxVel.x;
        }

        gameObject.rigidbody2D.velocity = vel;
	}
}
