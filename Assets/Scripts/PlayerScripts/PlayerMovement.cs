using UnityEngine;
using System.Collections;

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

    PlayerPowerups powerUps;
	// Use this for initialization
	void Start () 
    {

        Vector3 vel = gameObject.rigidbody2D.velocity;
        vel.x = VariableSpeed.current;
        gameObject.rigidbody2D.velocity = vel;
        jumpDelay = true;
        playerSprite = gameObject.GetComponentInChildren<SpriteRenderer>();
        playerSprite.color = playerColour;
        playerSprite.sortingOrder = Random.Range(0, 10);

        powerUps = gameObject.GetComponent<PlayerPowerups>();

        animationBoard.Fall();
        animationBoard.FlappyMode = false;
	    animationBoard.IntroMode = false;

        switch (LevelTypeManager.currentLevel)
        {
            case LevelTypeManager.Level.lowGravity:
                gameObject.rigidbody2D.gravityScale = lowGravityScale;
                jumpVel = lowGravityJumpVel;
                break;
            case LevelTypeManager.Level.flappyBird:
                gameObject.rigidbody2D.gravityScale = flappyGravityScale;
                animationBoard.FlappyMode = true;
                jumpTimeDelay = flappyJumpTimeDelay;
                jumpVel = flappyJumpVel;
                break;
            case LevelTypeManager.Level.gravityFlip:
                gameObject.rigidbody2D.gravityScale = flipGravityScale;
                animationBoard.FlappyMode = false;
                jumpVel = gravFlipJumpVel;
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

	// Update is called once per frame
	void FixedUpdate () 
    {
        Vector3 vel = gameObject.rigidbody2D.velocity;
        vel += rightAcceleration*Time.deltaTime;

        //Vector2 position = (Vector2)transform.position;
        Vector2 position = (Vector2)playerSprite.bounds.center;

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
                Debug.Log("landed");
                animationBoard.Land();
                //calledFalling = false;
            }
        }
        else
        {
            Debug.Log(vel.y);
            if (LevelTypeManager.currentLevel==LevelTypeManager.Level.gravityFlip)
            {
                animationBoard.Fall();
                Debug.Log("falling");
            }
            else if (vel.y < 0)
            {
                if (!calledFalling)
                {
                    Debug.Log("falling");
                    animationBoard.Fall();
                    calledFalling = true;
                }
            }
        }
        vel = jumpLogic(vel, onTheGround);
        vel = doubleJumpLogic(vel, onTheGround);

        if (vel.x >= VariableSpeed.current + tempSpeedBoost)
        {
            vel.x = VariableSpeed.current + tempSpeedBoost;
        }
                
        gameObject.rigidbody2D.velocity = vel;
        
        if (transform.position.x > Camera.main.transform.position.x)
        {
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
                //PowerupSounds.inst.playBoostJump();
                LevelSounds.inst.playPowerup(transform.position);
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
        if (Input.GetKey(playerKey) &&
            canDoubleJump &&
            jumpDelay &&
            jumpReleased)
        {
            vel.y = 0;
            if (powerUps.currentPowerUp == PlayerPowerups.PowerUp.doubleJump)
            {
                vel.y += jumpVel * Time.deltaTime;
                //PowerupSounds.inst.playDoubleJump();
                animationBoard.Jump();
            }
            else if (powerUps.currentPowerUp == PlayerPowerups.PowerUp.glide)
            {
                gameObject.rigidbody2D.gravityScale = 0;
                Invoke("resetGravity", jumpGlideTime);
                //PowerupSounds.inst.playGlide();
            }

            LevelSounds.inst.playPowerup(transform.position);

            canDoubleJump = false;
            jumpDelay = false;
            Invoke("resetJumpTImer", jumpTimeDelay);
            jumpReleased = false;

            //int rnd = Random.Range(0, SoundManager.instance.jumpSounds.Count - 1);
            //audio.PlayOneShot(SoundManager.instance.jumpSounds[rnd], SoundManager.instance.jumpVolume);
        }
        return vel;
    }
    
    void flipGravity()
    {
        flipGravityScale = -flipGravityScale;
        gameObject.rigidbody2D.gravityScale = flipGravityScale;
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
        gameObject.rigidbody2D.gravityScale = flipGravityScale;
    }

	public void setJumpKey(KeyCode key)
	{
		playerKey = key;
	}

}
