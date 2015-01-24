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
    bool calledFalling = false;
    public KeyCode playerKey;
    public Color playerColour;

    public AnimationBoard animationBoard;
    public SpriteRenderer playerSprite;

    public float raycastLength = 3.0f;
    public float raycastLengthRight = 3.0f;
    public LayerMask mask;
    float gravityScale = 0.0f;
    public float jumpGlideTime = 0.4f;
    GameObject obj = null;

    public enum PowerUp
    {
        none,
        speedUp,
        doubleJump,
        jumpBoost,
        glide,
        smash
    };
    public PowerUp powerUp;


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
        gravityScale = gameObject.rigidbody2D.gravityScale;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        Vector3 vel = gameObject.rigidbody2D.velocity;
        vel += rightAcceleration*Time.deltaTime;

        Vector2 position = (Vector2)transform.position;
        Vector2 down = -Vector2.up * raycastLength;
        Debug.DrawRay(transform.position, down, Color.green);
        RaycastHit2D hit = Physics2D.Raycast(position, down, raycastLength, ~mask.value);

        Vector2 right = Vector2.right * raycastLengthRight;
        Debug.DrawRay(transform.position, right, Color.green);
        RaycastHit2D hitFront = Physics2D.Raycast(position, right, raycastLengthRight, ~mask.value);

        bool infront = true;
        if (hitFront.collider == null)
        {
            infront = false;
        }

        if (infront)
        {
            Debug.Log(hitFront.collider.gameObject.tag);
            if (hitFront.collider.gameObject.tag == "obstacle" && obj != hitFront.collider.gameObject)
            {
                RandomShake.randomShake.PlaySinShake();
                obj = hitFront.collider.gameObject;
            }

            if (powerUp == PowerUp.smash)
            {
                Debug.Log("obstacle1");
                if (hitFront.collider.gameObject.tag == "obstacle")
                {
                    Debug.Log("obstacle2");
                    GameObject itemGenerator = GameObject.Find("ItemGenerator");
                    GenerateItems igScript = itemGenerator.GetComponent<GenerateItems>();
                    igScript.smashRock(hitFront.collider.gameObject);
                    powerUp = PowerUp.none;
                }
            }
        }


        bool onTheGround = true;
        if (hit.collider == null)
        {
            onTheGround = false;
        }


        if (onTheGround)
        {
            //print("There is something below the object!");
            canDoubleJump = true;
            jumpReleased = false;
            calledFalling = false;

            if (inTheAir)
            {
                animationBoard.Land();
                inTheAir = false;
            }
        }
        else if (inTheAir)
        {
            //Debug.Log(vel.y);
            if (vel.y <= 0)
            {
                //Debug.Log("falling");
                if (!calledFalling)
                {
                    animationBoard.Fall();
                    calledFalling = true;
                }
            }
        }

        if (Input.GetKey(KeyCode.Return))
        {
            powerUp = PowerUp.smash;
        }

        vel = jumpLogic(vel, onTheGround);
        vel = doubleJumpLogic(vel, onTheGround);
        vel = glideJumpLogic(vel, onTheGround);

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
        else if (tag == "glide")
        {
            powerUp = PowerUp.glide;
        }
        else if (tag == "smash")
        {
            powerUp = PowerUp.smash;
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
            if (powerUp == PowerUp.doubleJump ||
                powerUp == PowerUp.glide)
                Invoke("resetJumpTImer", jumpTimeDelay / 4);
            else
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
            //Debug.Log(jumpReleased);
        }
        if (Input.GetKey(playerKey) &&
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

    Vector3 glideJumpLogic(Vector3 vel, bool onTheGround)
    {
        if (Input.GetKey(playerKey) &&
            powerUp == PowerUp.glide &&
            canDoubleJump &&
            jumpDelay &&
            jumpReleased)
        {
            Debug.Log("Glide!");
            vel.y = 0;

            canDoubleJump = false;
            jumpDelay = false;

            Invoke("resetGravity", jumpGlideTime);
            gameObject.rigidbody2D.gravityScale = 0;
            Invoke("resetJumpTImer", jumpTimeDelay);
            jumpReleased = false;
            inTheAir = true;
        }
        return vel;
    }

    void resetGravity()
    {
        gameObject.rigidbody2D.gravityScale = gravityScale;
    }

	public void setJumpKey(KeyCode key)
	{
		playerKey = key;
	}
}
