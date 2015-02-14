using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class PlayerMovement : MonoBehaviour 
{
    Vector3 moveVel = Vector3.zero;
    public Vector3 rightAcceleration = Vector3.zero;
    public Vector3 maxVel = Vector3.zero;
    public float jumpVel = 0.0f;
    public float jumpBoostVel = 0.0f;
    public float jumpTimeDelay = 0.4f;
    float tempSpeedBoost = 0.0f;
    bool jumpDelay = true;
    bool canDoubleJump = false;
    bool jumpReleased = false;
    bool onTheGroundLast = false;
    bool onTheGround = false;
    //bool inTheAir = false;
    bool calledFalling = false;
    public KeyCode playerKey;
    public Color playerColour;


    public AnimationBoard animationBoard;
    SpriteRenderer playerSprite;

    public float raycastLength = 3.0f;
    public float raycastLengthRight = 3.0f;
    public LayerMask mask;
    public bool canFootstep = true;
    public float jumpGlideTime = 0.4f;
    GameObject obj = null;


    public GameObject hitEffect;
    public GameObject rockEffect;

    public float gravityScale = 8;
    public float lowGravityScale = 8;
    PlayerPowerups powerUps;
	// Use this for initialization
	void Start () 
    {
        mLastCollidedObstacle = null;

        Vector3 vel = gameObject.rigidbody2D.velocity;
        vel.x = VariableSpeed.current;
        gameObject.rigidbody2D.velocity = vel;
        jumpDelay = true;
        playerSprite = gameObject.GetComponentInChildren<SpriteRenderer>();
        playerSprite.color = playerColour;
        playerSprite.sortingOrder = Random.Range(0, 10);

        powerUps = gameObject.GetComponent<PlayerPowerups>();

        animationBoard.FlappyMode = false;
        switch (LevelTypeManager.currentLevel)
        {
            case LevelTypeManager.Level.lowGravity:
                gameObject.rigidbody2D.gravityScale = lowGravityScale;
                jumpVel = jumpVel/1.5f;
                break;
            case LevelTypeManager.Level.flappyBird:
                gameObject.rigidbody2D.gravityScale = gravityScale;
                animationBoard.FlappyMode = true;
                break;
            case LevelTypeManager.Level.gravityFlip:
                gameObject.rigidbody2D.gravityScale = gravityScale;
                animationBoard.FlappyMode = false;
                jumpVel = jumpVel/8;
                break;
            default:
                gameObject.rigidbody2D.gravityScale = gravityScale;
                break;
        }
	}
    Vector2 rayDownDir = -Vector2.up;
    float flipAngle = 180;
	void canFootstepReset()
    {
        canFootstep = true;
    }

    bool rayCastDown()
    {
        return rayCastDown(Vector2.zero);
    }

    bool rayCastDown(Vector2 offset)
    {
        Vector2 dir = rayDownDir;
        return rayCastFromPlayer(offset, dir, raycastLength);
    }

    bool rayCastForward()
    {
        return rayCastForward(Vector2.zero);
    }
    bool rayCastForward(Vector2 offset)
    {
        Vector2 dir = Vector2.right;
        return rayCastFromPlayer(offset, dir, raycastLength);
    }
    bool rayCastFromPlayer(Vector2 offset, Vector2 dir, float rayLength)
    {
        Vector2 position = (Vector2)playerSprite.bounds.center;
        Debug.DrawRay(position, dir * rayLength, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(position, dir, rayLength, ~mask.value);
        if (hit.collider != null)
        {
            return true;
        }
        return false;
    }

	// Update is called once per frame
	void FixedUpdate () 
    {
        Vector3 vel = gameObject.rigidbody2D.velocity;
        vel += rightAcceleration*Time.deltaTime;

        //Vector2 position = (Vector2)transform.position;
        Vector2 position = (Vector2)playerSprite.bounds.center;

        bool infront = false;
        Vector2 right = Vector2.right * raycastLengthRight;
        Debug.DrawRay(transform.position, right, Color.green);
        RaycastHit2D hitFront = Physics2D.Raycast(position, right, raycastLengthRight, ~mask.value);

        if (hitFront.collider != null)
        {
            infront = true;
        }

        onTheGround = false;
        Vector2 xOffset = Vector2.one / 2;
        xOffset.y = 0;

        if(rayCastDown())
        {
            onTheGround = true;
        }
        else if (rayCastDown(xOffset))
        {
            onTheGround = true;
        }
        else if (rayCastDown(-xOffset))
        {
            onTheGround = true;
        }

        if (infront)
        {
            //Debug.Log(hitFront.collider.gameObject.tag);
            if( obj != hitFront.collider.gameObject)
            {
                if (hitFront.collider.gameObject.tag == "obstacle" || hitFront.collider.gameObject.tag == "deadplayer")
                {
                    mLastCollidedObstacle = hitFront.collider.gameObject;

                    RandomShake.randomShake.PlaySinShake();
                    Instantiate(hitEffect,
                        new Vector3(hitFront.point.x, hitFront.point.y, hitEffect.transform.position.z),
                        hitEffect.transform.rotation);
                    obj = hitFront.collider.gameObject;

                    if (powerUps.currentPowerUp != PlayerPowerups.PowerUp.smash)
                    {
                        int rnd = Random.Range(0, SoundManager.instance.hitSounds.Count);
                        audio.PlayOneShot(SoundManager.instance.hitSounds[rnd], SoundManager.instance.hitVolume);
                    }
                }
            }

            if (powerUps.currentPowerUp == PlayerPowerups.PowerUp.smash)
            {
                //Debug.Log("obstacle1");
                if (hitFront.collider.gameObject.tag == "obstacle")
                {
                    //Debug.Log("obstacle2");
                    GameObject itemGenerator = GameObject.Find("ItemGenerator");
                    GenerateItems igScript = itemGenerator.GetComponent<GenerateItems>();
                    igScript.smashRock(hitFront.collider.gameObject);

                    Instantiate(rockEffect,
                        new Vector3(hitFront.point.x, hitFront.point.y, hitEffect.transform.position.z),
                        rockEffect.transform.rotation);
                    obj = hitFront.collider.gameObject;
                    animationBoard.Hit();
                    PowerupSounds.inst.playSmash();
                    powerUps.decrementCharges();
                }
            }
        }



        if (onTheGround)
        {
            canDoubleJump = true;
            jumpReleased = false;
            calledFalling = false;

            if (canFootstep)
            {
                int rnd = Random.Range(0, SoundManager.instance.footstepSounds.Count);
                audio.PlayOneShot(SoundManager.instance.footstepSounds[rnd], SoundManager.instance.footstepVolume);
                canFootstep = false;
                Invoke("canFootstepReset", SoundManager.instance.footstepSounds[rnd].length);
            }

            if (!onTheGroundLast)
            {
                animationBoard.Land();
                calledFalling = false;
            }
        }
        else
        {
            if (LevelTypeManager.currentLevel==LevelTypeManager.Level.gravityFlip)
            {
                animationBoard.Fall();
            }
            else if (vel.y <= 0)
            {
                if (!calledFalling)
                {
                    animationBoard.Fall();
                    calledFalling = true;
                }
            }
        }
        vel = jumpLogic(vel, onTheGround);
        vel = doubleJumpLogic(vel, onTheGround);
        vel = glideJumpLogic(vel, onTheGround);

        //if (vel.x >=   maxVel.x)
        //{
        //    vel.x = maxVel.x;
        //}
        if (vel.x >= VariableSpeed.current + tempSpeedBoost)
        {
            vel.x = VariableSpeed.current + tempSpeedBoost;
        }

        
        gameObject.rigidbody2D.velocity = vel;


        if (transform.position.x > Camera.main.transform.position.x)
        {
            //Debug.Log(transform.position.x);
            //Debug.Log(Camera.main.transform.position.x);

            SpriteRenderer sr = gameObject.GetComponentInChildren<SpriteRenderer>();

            Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, 0.0f, 0.0f));
            Vector3 size = sr.renderer.bounds.max - sr.renderer.bounds.min;
            worldPos.x -= size.x;
            worldPos.y = transform.position.y;
            worldPos.z = transform.position.z;

            if (transform.position.x>worldPos.x)
            {
                transform.position = worldPos;
            }
        }
        onTheGroundLast = onTheGround;
	}

    void resetJumpTImer()
    {
        jumpDelay = true;
    }
    public void Update()
    {
        
    }

    public void speedBoost()
    {
        tempSpeedBoost += VariableSpeed.currentBoost;
        Invoke("resetTempSpeedBoost", VariableSpeed.currentSpeedBoostTime);
    }
    
    

    void resetTempSpeedBoost()
    {
        tempSpeedBoost = 0.0f;
    }

    Vector3 jumpLogic(Vector3 vel, bool onTheGround)
    {
        if (Input.GetKey(playerKey) &&
            onTheGround &&
            jumpDelay)
        {
            vel.y += jumpVel * Time.deltaTime;
            animationBoard.Jump();

            if (powerUps.currentPowerUp == PlayerPowerups.PowerUp.jumpBoost)
            {
                vel.y += jumpBoostVel * Time.deltaTime;

                PowerupSounds.inst.playBoostJump();
            }

            jumpDelay = false;
            if (powerUps.currentPowerUp == PlayerPowerups.PowerUp.doubleJump ||
                powerUps.currentPowerUp == PlayerPowerups.PowerUp.glide)
                Invoke("resetJumpTImer", jumpTimeDelay / 4);
            else
                Invoke("resetJumpTImer", jumpTimeDelay);
            jumpReleased = false;

            if (powerUps.currentPowerUp != PlayerPowerups.PowerUp.jumpBoost)
            {
                int rnd = Random.Range(0, SoundManager.instance.jumpSounds.Count);
                audio.PlayOneShot(SoundManager.instance.jumpSounds[rnd], SoundManager.instance.jumpVolume);
            }

            if (LevelTypeManager.currentLevel == LevelTypeManager.Level.gravityFlip)
            {
                Debug.Log("Flip gravity");
                flipGravity();
            }
        }
        return vel;
    }
    
    Vector3 doubleJumpLogic(Vector3 vel, bool onTheGround)
    {
        if (!onTheGround && !Input.GetKey(playerKey))
        {
            jumpReleased = true;
        }
        if (Input.GetKey(playerKey) &&
            powerUps.currentPowerUp == PlayerPowerups.PowerUp.doubleJump &&
            canDoubleJump &&
            jumpDelay &&
            jumpReleased)
        {
            vel.y = 0;
            vel.y += jumpVel * Time.deltaTime;
            
            canDoubleJump = false;
            jumpDelay = false;
            Invoke("resetJumpTImer", jumpTimeDelay);
            jumpReleased = false;

            PowerupSounds.inst.playDoubleJump();
            animationBoard.Jump();
            //int rnd = Random.Range(0, SoundManager.instance.jumpSounds.Count - 1);
            //audio.PlayOneShot(SoundManager.instance.jumpSounds[rnd], SoundManager.instance.jumpVolume);
        }
        return vel;
    }

    Vector3 glideJumpLogic(Vector3 vel, bool onTheGround)
    {
        if (Input.GetKey(playerKey) &&
            powerUps.currentPowerUp == PlayerPowerups.PowerUp.glide &&
            canDoubleJump &&
            jumpDelay &&
            jumpReleased)
        {
            //Debug.Log("Glide!");
            vel.y = 0;

            canDoubleJump = false;
            jumpDelay = false;

            Invoke("resetGravity", jumpGlideTime);
            gameObject.rigidbody2D.gravityScale = 0;
            Invoke("resetJumpTImer", jumpTimeDelay);
            jumpReleased = false;

            PowerupSounds.inst.playGlide();

            //int rnd = Random.Range(0, SoundManager.instance.jumpSounds.Count - 1);
            //audio.PlayOneShot(SoundManager.instance.jumpSounds[rnd], SoundManager.instance.jumpVolume);
        }
        return vel;
    }

    void flipGravity()
    {
        gravityScale = -gravityScale;
        gameObject.rigidbody2D.gravityScale = gravityScale;
        jumpVel = -jumpVel;
        rayDownDir = -rayDownDir;

        transform.rotation = Quaternion.AngleAxis(flipAngle, Vector3.forward);
        if (flipAngle == 180)
        {
            flipAngle = 0;
        }
        else
        {
            flipAngle = 180;
        }
    }

    void resetGravity()
    {
        gameObject.rigidbody2D.gravityScale = gravityScale;
    }

	public void setJumpKey(KeyCode key)
	{
		playerKey = key;
	}

    public GameObject getLastCollidedObstacle() {
        return mLastCollidedObstacle;
    }

    private GameObject mLastCollidedObstacle;
}
