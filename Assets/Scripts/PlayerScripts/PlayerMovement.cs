using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class PlayerMovement : MonoBehaviour 
{
    Vector3 moveVel = Vector3.zero;
    public Vector3 rightAcceleration = Vector3.zero;
    public Vector3 maxVel = Vector3.zero;
    public float jumpBoostVel = 0.0f;
    public float jumpTimeDelay = 0.4f;
    public float flappyJumpTimeDelay = 0.4f;
    float tempSpeedBoost = 0.0f;
    bool jumpDelay = true;
    bool landDelay = true;
    bool canDoubleJump = false;
    bool jumpReleased = false;
    bool onTheGroundLast = false;
    bool onTheGround = false;
    bool calledFalling = false;
    public KeyCode playerKey;
    public Color playerColour;


    public AnimationBoard animationBoard;
    SpriteRenderer playerSprite;

    public float raycastLength = 3.0f;
    public LayerMask mask;
    public bool canFootstep = true;
    public float jumpGlideTime = 0.4f;


    public float gravityScale = 8;
    public float lowGravityScale = 8;
    public float flappyGravityScale = 8;
    public float flipGravityScale = 8;

    public float jumpVel = 0.0f;
    public float flappyJumpVel = 0.0f;
    public float lowGravityJumpVel = 0.0f;
    public float gravFlipJumpVel = 0.0f;

    private bool mBtnPressed = false;
    private bool letJumpGo = true;

    private int bunnies_murdered = 0;
    private int pots_smashed = 0;
    private int planks_broken = 0;
    private int num_jumps = 0;
    private int powerups_acquired = 0;

    PlayerPowerups powerUps;
    void Awake()
    {
        powerUps = gameObject.GetComponent<PlayerPowerups>();
    }

	// Use this for initialization
	void Start () 
    {
        Vector3 vel = gameObject.GetComponent<Rigidbody2D>().velocity;
        vel.x = VariableSpeed.current;
        gameObject.GetComponent<Rigidbody2D>().velocity = vel;
        jumpDelay = true;
        playerSprite = gameObject.GetComponentInChildren<SpriteRenderer>();
        playerSprite.color = playerColour;
        playerSprite.sortingOrder = Random.Range(0, 10);

        powerUps.createPowerupEffect();

        animationBoard.FlappyMode = false;
	    animationBoard.IntroMode = false;

        switch (LevelTypeManager.currentLevel)
        {
            case LevelTypeManager.Level.lowGravity:
                gameObject.GetComponent<Rigidbody2D>().gravityScale = lowGravityScale;
                jumpVel = lowGravityJumpVel;
                break;
            case LevelTypeManager.Level.flappyBird:
                gameObject.GetComponent<Rigidbody2D>().gravityScale = flappyGravityScale;
                animationBoard.FlappyMode = true;
                jumpTimeDelay = flappyJumpTimeDelay;
                jumpVel = flappyJumpVel;
                break;
            case LevelTypeManager.Level.gravityFlip:
                gameObject.GetComponent<Rigidbody2D>().gravityScale = flipGravityScale;
                animationBoard.FlappyMode = false;
                jumpVel = gravFlipJumpVel;
                break;
            default:
                gameObject.GetComponent<Rigidbody2D>().gravityScale = gravityScale;
                break;
        }
        animationBoard.Fall();

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
    public bool rayCastFromPlayer(Vector2 offset, Vector2 dir, float rayLength)
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

    public void incPlanks() {
        planks_broken++;
    }

    public void incBunnies() {
        bunnies_murdered++;
    }

    public void incPots() {
        pots_smashed++;
    }

    public int planksBroken() {
        return planks_broken;
    }

    public int bunniesMurdered() {
        return bunnies_murdered;
    }

    public int potsSmashed() {
        return pots_smashed;
    }

    public int numJumps() {
        return num_jumps;
    }

    public void incPowerups() {
        powerups_acquired++;
    }

    public int powerupsAcquired() {
        return powerups_acquired++;
    }

	// Update is called once per frame
	void FixedUpdate () 
    {
        Vector3 vel = gameObject.GetComponent<Rigidbody2D>().velocity;
        vel += rightAcceleration*Time.deltaTime;

        Vector3 position = transform.position;
        if (position.y < -10 || position.y >10)
        {
            position.y = 0;
            transform.position = position;
        }

        onTheGround = false;
        Vector2 xOffset = Vector2.one / 2;
        xOffset.y = 0;


        for (int i = -1; i < 2;i++)
        {
            if (rayCastDown(xOffset*i))
            {
                onTheGround = true;
                break;
            }
        }
        
        if (onTheGround)
        {
            canDoubleJump = true;
            jumpReleased = false;
            calledFalling = false;

            if (canFootstep)
            {
                float length = LevelSounds.inst.playFootstep(gameObject.transform.position);
                
                canFootstep = false;
                Invoke("canFootstepReset", length);
            }

            if (!onTheGroundLast)
            {
                animationBoard.Land();                
            }
        }
        else
        {
            animationBoard.setGrounded(false);
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
                    Invoke("landDelay",0.01f);
                }
            }
        }
        vel = jumpLogic(vel, onTheGround);
        vel = doubleJumpLogic(vel, onTheGround);

        if (vel.x >= VariableSpeed.current + tempSpeedBoost)
        {
            vel.x = VariableSpeed.current + tempSpeedBoost;
        }
                
        gameObject.GetComponent<Rigidbody2D>().velocity = vel;
        
        if (transform.position.x > Camera.main.transform.position.x)
        {
            SpriteRenderer sr = gameObject.GetComponentInChildren<SpriteRenderer>();

            Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, 0.0f, 0.0f));
            Vector3 size = sr.GetComponent<Renderer>().bounds.max - sr.GetComponent<Renderer>().bounds.min;
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
    void resetLandTimer()
    {
        landDelay = true;
    }
    void resetJumpTImer()
    {
        jumpDelay = true;
    }
    //public void Update()
    //{
        
    //}

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
        //check if the player jumped in the round
        if(Input.GetKey(playerKey))
            mBtnPressed = true;

        if (Input.GetKeyUp(playerKey))
        {
            letJumpGo = true;
        }

        if (Input.GetKey(playerKey) &&
            onTheGround &&
            //letJumpGo &&
            jumpDelay &&
            animationBoard.canJump())
        {
            if(LevelTypeManager.Level.flappyBird != LevelTypeManager.currentLevel)
                num_jumps++;
            
            vel.y += jumpVel * Time.deltaTime;
            animationBoard.Jump();

            if (powerUps.currentPowerUp == PlayerPowerups.PowerUp.jumpBoost)
            {
                vel.y += jumpBoostVel * Time.deltaTime;
                //PowerupSounds.inst.playBoostJump();
                LevelSounds.inst.playPowerup(transform.position);
            }
            jumpDelay = false;
            letJumpGo = false;
            //landDelay = false;
            if (powerUps.currentPowerUp == PlayerPowerups.PowerUp.doubleJump ||
                powerUps.currentPowerUp == PlayerPowerups.PowerUp.glide)
                Invoke("resetJumpTImer", jumpTimeDelay / 4);
            else
                Invoke("resetJumpTImer", jumpTimeDelay);
            jumpReleased = false;

            if (powerUps.currentPowerUp != PlayerPowerups.PowerUp.jumpBoost)
            {
                LevelSounds.inst.playJump(transform.position);
            }

            if (LevelTypeManager.currentLevel == LevelTypeManager.Level.gravityFlip)
            {
                Debug.Log("Flip gravity");
                flipGravity();
            }
        }
        else if (Input.GetKey(playerKey) &&
            jumpDelay &&
            LevelTypeManager.currentLevel == LevelTypeManager.Level.flappyBird)
        {
            vel.y = 0;
            vel.y += jumpVel * Time.deltaTime;
            animationBoard.Jump();
            Invoke("resetJumpTImer", jumpTimeDelay);
            jumpDelay = false;
            //landDelay = false;
            LevelSounds.inst.playJump(transform.position);
        }
        return vel;
    }
    
    Vector3 doubleJumpLogic(Vector3 vel, bool onTheGround)
    {
        if (!onTheGround && !Input.GetKey(playerKey))
        {
            jumpReleased = true;
        }
        if (powerUps.currentPowerUp != PlayerPowerups.PowerUp.doubleJump &&
            powerUps.currentPowerUp != PlayerPowerups.PowerUp.glide)
            return vel;

        if (Input.GetKey(playerKey) &&
            canDoubleJump &&
            jumpDelay &&
            landDelay &&
            jumpReleased
            )
        {

            vel.y = 0;
            if (powerUps.currentPowerUp == PlayerPowerups.PowerUp.doubleJump)
            {
                vel.y += jumpVel * Time.deltaTime;
                animationBoard.Jump();
            }
            else if (powerUps.currentPowerUp == PlayerPowerups.PowerUp.glide)
            {
                gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
                Invoke("resetGravity", jumpGlideTime);
            }

            LevelSounds.inst.playPowerup(transform.position);

            canDoubleJump = false;
            jumpDelay = false;
            //landDelay = false;
            Invoke("resetJumpTImer", jumpTimeDelay);
            jumpReleased = false;

        }
        return vel;
    }
    
    void flipGravity()
    {
        flipGravityScale = -flipGravityScale;
        gameObject.GetComponent<Rigidbody2D>().gravityScale = flipGravityScale;
        jumpVel = -jumpVel;
        rayDownDir = -rayDownDir;

        transform.rotation = Quaternion.AngleAxis(flipAngle, Vector3.forward);
        transform.localScale = new Vector3(-transform.localScale.x,transform.localScale.y,transform.localScale.z);
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
        gameObject.GetComponent<Rigidbody2D>().gravityScale = flipGravityScale;
    }

	public void setJumpKey(KeyCode key)
	{
		playerKey = key;
	}

    void OnDestroy() {
        if(!mBtnPressed) {
            CurrentPlayerKeys.Instance.removePlayer(new KeyValuePair<KeyCode, Color>(playerKey, playerColour));
        }
    }
}
