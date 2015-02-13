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
    bool inTheAir = false;
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

        ParticleSystem ps = gameObject.GetComponentInChildren<ParticleSystem>();
        ps.startColor = playerColour;


        switch (LevelTypeManager.currentLevel)
        {
            case LevelTypeManager.Level.lowGravity:
                gameObject.rigidbody2D.gravityScale = lowGravityScale;
                jumpVel = jumpVel/1.5f;
                break;
            default:
                gameObject.rigidbody2D.gravityScale = gravityScale;
                break;
        }
        if (LevelTypeManager.currentLevel == LevelTypeManager.Level.flappyBird)
        {
            animationBoard.FlappyMode = true;
        }
        else
        {
            animationBoard.FlappyMode = false;
        }
	}

	void canFootstepReset()
    {
        canFootstep = true;
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

        bool onTheGround = false;
        Vector2 xOffset = Vector2.one / 2;
        xOffset.y = 0;

        Vector2 down = -Vector2.up * raycastLength;
        Debug.DrawRay(position, down, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(position, down, raycastLength, ~mask.value);
        if (hit.collider != null)
        {
            onTheGround = true;
        }
        Debug.DrawRay(position + xOffset, down, Color.yellow);
        hit = Physics2D.Raycast(position + xOffset, down, raycastLength, ~mask.value);
        if (hit.collider != null)
        {
            onTheGround = true;
        }
        Debug.DrawRay(position - xOffset, down, Color.magenta);
        hit = Physics2D.Raycast(position - xOffset, down, raycastLength, ~mask.value);
        if (hit.collider != null)
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
            //print("There is something below the object!");
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

            if (inTheAir)
            {
                animationBoard.Land();
                inTheAir = false;
            }
        }
        else
        {
            inTheAir = true;
        }

        if (inTheAir)
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


        //if (Input.GetKey(KeyCode.Return))
        //{
        //    powerUp = PowerUp.smash;
        //}

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
            inTheAir = true;

            if (powerUps.currentPowerUp != PlayerPowerups.PowerUp.jumpBoost)
            {
                int rnd = Random.Range(0, SoundManager.instance.jumpSounds.Count);
                audio.PlayOneShot(SoundManager.instance.jumpSounds[rnd], SoundManager.instance.jumpVolume);
            }
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
            powerUps.currentPowerUp == PlayerPowerups.PowerUp.doubleJump &&
            canDoubleJump &&
            jumpDelay &&
            jumpReleased)
        {
            vel.y = 0;
            vel.y += jumpVel * Time.deltaTime;

            //if (powerUp == PowerUp.jumpBoost)
            //{
            //    vel.y += jumpBoostVel * Time.deltaTime;
            //}

            canDoubleJump = false;
            jumpDelay = false;
            Invoke("resetJumpTImer", jumpTimeDelay);
            jumpReleased = false;
            inTheAir = true;

            PowerupSounds.inst.playDoubleJump();

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
            inTheAir = true;

            PowerupSounds.inst.playGlide();

            //int rnd = Random.Range(0, SoundManager.instance.jumpSounds.Count - 1);
            //audio.PlayOneShot(SoundManager.instance.jumpSounds[rnd], SoundManager.instance.jumpVolume);
        }
        return vel;
    }

    void flipGravity()
    {
        gameObject.rigidbody2D.gravityScale = -gravityScale;
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
