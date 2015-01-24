using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour 
{
    Vector3 moveVel = Vector3.zero;
    public Vector3 rightAcceleration = Vector3.zero;
    public Vector3 maxVel = Vector3.zero;
    public float jumpVel = 0.0f;

    public float raycastLength = 3.0f;
    public LayerMask mask;
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
        
        Vector3 vel = gameObject.rigidbody2D.velocity;
        vel += rightAcceleration*Time.deltaTime;

        Vector2 position = (Vector2)transform.position;
        Vector2 down = -Vector2.up * raycastLength;
        Debug.DrawRay(transform.position, down, Color.green);
        //bool onTheGround = Physics.Raycast(transform.position, Vector3.down, raycastLength);
        RaycastHit2D hit;
        //Ray2D ray = new Ray2D(transform.position, down);
        //rigidbody2D.

        bool onTheGround = Physics2D.Raycast(position, down, raycastLength,~mask.value);


        if (onTheGround)
        {
            print("There is something below the object!");
        }
        if (Input.GetKey(KeyCode.UpArrow) && onTheGround)
        {


            vel.y += jumpVel * Time.deltaTime;

        }

        if (vel.x >= maxVel.x)
        {
            vel.x = maxVel.x;
        }

        gameObject.rigidbody2D.velocity = vel;
	}
}
