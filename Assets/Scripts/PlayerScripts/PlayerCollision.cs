using UnityEngine;
using System.Collections;

public class PlayerCollision : MonoBehaviour
{
    public GameObject hitEffect;
    public GameObject rockEffect;

    public float raycastLengthRight = 0.4f;
    private GameObject mLastCollidedObstacle;
    GameObject obj = null;
    PlayerPowerups powerUps;
    PlayerMovement playerMovement;
    public GameObject getLastCollidedObstacle()
    {
        return mLastCollidedObstacle;
    }

    //public float raycastLength = 0.4f;
    void Start()
    {
        mLastCollidedObstacle = null;
        powerUps = gameObject.GetComponent<PlayerPowerups>();
        playerMovement = gameObject.GetComponent<PlayerMovement>();
    }
    void FixedUpdate()
    {
        bool infront = false;
        Vector2 position = (Vector2)gameObject.transform.position;

        Vector2 right = Vector2.right * raycastLengthRight;
        Debug.DrawRay(transform.position, right, Color.green);
        RaycastHit2D hitFront = Physics2D.Raycast(position, right, raycastLengthRight, ~playerMovement.mask.value);

        if (hitFront.collider != null)
        {
            infront = true;
        }
        if (infront)
        {
            //Debug.Log(hitFront.collider.gameObject.tag);
            if (obj != hitFront.collider.gameObject)
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
                    playerMovement.animationBoard.Hit();
                    //PowerupSounds.inst.playSmash();
                    LevelSounds.inst.playPowerup(transform.position);
                    powerUps.decrementCharges();
                }
            }
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {        
        if (collision.collider.gameObject.tag == "obstacle")
        {
            RandomShake.randomShake.PlaySinShake();
        }
    }
}