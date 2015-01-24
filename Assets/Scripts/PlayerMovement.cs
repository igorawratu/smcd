using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour 
{
    Vector3 moveVel = Vector3.zero;
    public Vector3 rightAcceleration = Vector3.zero;
    public Vector3 maxVel = Vector3.zero;
    public float jumpVel = 0.0f;
    public float jumpBoostVel = 0.0f;
    public float jumpTimeDelay = 0.4f;
    bool jumpDelay = true;
    bool canDoubleJump = false;
    bool jumpReleased = false;
    bool inTheAir = false;
    public KeyCode playerKey;
    public Color playerColour;

    public AnimationBoard animationBoard;
    public SpriteRenderer playerSprite;

    public float raycastLength = 3.0f;
    public LayerMask mask;

    enum PowerUp
    {
        none,
        speedUp,
        doubleJump,
        jumpBoost,
        glide
    };
    PowerUp powerUp;


	// Use this for initialization
	void Start () 
    {
        moveVel = CameraLogic.camLogic.moveVel;
        Vector3 vel = gameObject.rigidbody2D.velocity;
        vel.x += 10;
        gameObject.rigidbody2D.velocity = vel;
        powerUp = PowerUp.none;
        jumpDelay = true;
        SpriteRenderer sr = gameObject.GetComponentInChildren<SpriteRenderer>();
        sr.color = playerColour;
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
            //print("There is something below the object!");
            canDoubleJump = true;
            jumpReleased = false;

            if (inTheAir)
            {
                animationBoard.Land();
                inTheAir = false;
            }
        }
        else if (inTheAir)
        {
            if (vel.y <= 0)
            {
                animationBoard.Fall();
            }
        }

        if (Input.GetKey(KeyCode.Return))
        {
            powerUp = PowerUp.doubleJump;
        }

        vel = jumpLogic(vel, onTheGround);
        vel = doubleJumpLogic(vel, onTheGround);

        if (vel.x >= maxVel.x)
        {
            vel.x = maxVel.x;
        }

        gameObject.rigidbody2D.velocity = vel;
	}

    void resetJumpTImer()
    {
        jumpDelay = true;
    }

    public void ActivatePowerUp(string tag)
    {
        if(tag=="speedUp")
        {
            powerUp = PowerUp.speedUp;
        }
        else if (tag == "jumpBoost")
        {
            powerUp = PowerUp.jumpBoost;
        }
        else if (tag == "doubleJump")
        {
            powerUp = PowerUp.doubleJump;
        }
    }

    Vector3 jumpLogic(Vector3 vel, bool onTheGround)
    {
        if (Input.GetKey(playerKey) &&
            onTheGround &&
            jumpDelay)
        {

            vel.y += jumpVel * Time.deltaTime;
            animationBoard.Jump();

            if (powerUp == PowerUp.jumpBoost)
                vel.y += jumpBoostVel * Time.deltaTime;

            jumpDelay = false;
            if (powerUp == PowerUp.doubleJump)
                Invoke("resetJumpTImer", jumpTimeDelay / 4);


            Invoke("resetJumpTImer", jumpTimeDelay);
            jumpReleased = false;
            inTheAir = true;
        }
        return vel;
    }

    Vector3 doubleJumpLogic(Vector3 vel, bool onTheGround)
    {
        if (!onTheGround && !Input.GetKey(playerKey))
        {
            jumpReleased = true;
        }

        if (Input.GetKey(KeyCode.UpArrow) &&
            powerUp == PowerUp.doubleJump &&
            canDoubleJump &&
            jumpDelay &&
            jumpReleased)
        {
            vel.y = 0;
            vel.y += jumpVel * Time.deltaTime;

            if (powerUp == PowerUp.jumpBoost)
                vel.y += jumpBoostVel * Time.deltaTime;

            canDoubleJump = false;
            jumpDelay = false;
            Invoke("resetJumpTImer", jumpTimeDelay);
            jumpReleased = false;
            inTheAir = true;
        }
        return vel;
    }

	public void setJumpKey(KeyCode key)
	{
		playerKey = key;
	}
}
