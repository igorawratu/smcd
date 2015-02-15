using UnityEngine;
using System.Collections;

public class PlayerPowerups : MonoBehaviour {

    public enum PowerUp
    {
        none,
        speedUp,
        doubleJump,
        jumpBoost,
        glide,
        smash
    };

    public PowerUp currentPowerUp
    {
        get
        {
            return powerup;
        }
        set
        {
            //powerup = value;
        }
    }

    private PowerUp powerup;
    PlayerMovement playerMovement;
    int charges = 0;
    public int numSmashCharges =1;

    public GameObject powerupEffect;

    public GameObject powerUpSprite;
    SpriteRenderer powerUpSpriteRenderer;


    public Color[] powerUpColours = new Color[4];

	// Use this for initialization
	void Start () {
        playerMovement = gameObject.GetComponent<PlayerMovement>();
        powerUpSpriteRenderer = powerUpSprite.GetComponentInChildren<SpriteRenderer>();
        powerup = PowerUp.none;
        powerUpSpriteRenderer.sortingOrder = Random.Range(0, 10);

        ParticleSystem ps = gameObject.GetComponentInChildren<ParticleSystem>();
        ps.startColor = playerMovement.playerColour;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (powerup == PowerUp.none)
        {
            //powerUpSpriteRenderer.gameObject.SetActive(false);
            powerUpSprite.SetActive(false);
        }
        else
        {
            powerUpSprite.SetActive(true);
        }
	}
    public void decrementCharges()
    {
        charges--;
        if (charges <= 0)
        {
            powerup = PowerUp.none;
            charges = 0;
        }
    }
    public void ActivatePowerUp(string tag)
    {
        switch (LevelTypeManager.currentLevel)
        {
            case LevelTypeManager.Level.standard:
                powerup = PowerUp.jumpBoost;
                break;
            case LevelTypeManager.Level.lowGravity:
                powerup = PowerUp.doubleJump;
                break;
            case LevelTypeManager.Level.flappyBird:
                powerup = PowerUp.smash;
                charges = numSmashCharges;
                break;
            case LevelTypeManager.Level.gravityFlip:
                powerup = PowerUp.glide;
                break;
        }

        playerMovement.speedBoost();

        createPowerupEffect();

        powerUpSpriteRenderer.color = powerUpColours[0];
        LevelSounds.inst.playPickup(transform.position);

        //switch (powerup)
        //{
        //    case PowerUp.doubleJump:
        //        powerUpSpriteRenderer.color = powerUpColours[0];
        //        PowerupSounds.inst.playDoubleJumpPickup();
        //        break;
        //    case PowerUp.glide:
        //        powerUpSpriteRenderer.color = powerUpColours[1];
        //        PowerupSounds.inst.playGlidePickup();
        //        break;
        //    case PowerUp.jumpBoost:
        //        powerUpSpriteRenderer.color = powerUpColours[2];
        //        PowerupSounds.inst.playBoostJumpPickup();
        //        break;
        //    case PowerUp.smash:
        //        powerUpSpriteRenderer.color = powerUpColours[3];
        //        PowerupSounds.inst.playSmashPickup();
        //        break;
        //}
    }


    public void createPowerupEffect()
    {
        GameObject powerupFX = (GameObject)Instantiate(powerupEffect);
        powerupFX.transform.position = new Vector3(transform.position.x, 1, 0);
        powerupFX.GetComponentsInChildren<SpriteRenderer>()[0].color = playerMovement.playerColour;
    }

}
