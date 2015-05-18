using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UpdatePowerup : MonoBehaviour {
    enum PowerupPattern
    {
        LR,
        UD,
        CIRCLE
    };

	// Use this for initialization
	void Start () {
        mPatterns = new PowerupPattern[2];
        mPatterns[0] = PowerupPattern.LR;
        mPatterns[1] = PowerupPattern.UD;
        System.Random rng = new System.Random();
        int pattern = rng.Next(0, 2);
        mPattern = mPatterns[pattern];
        center = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        origin = 0;

        if (mPattern == PowerupPattern.LR){
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(4, 0);
        }
        else if (mPattern == PowerupPattern.UD)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 2);
        }
        else if (mPattern == PowerupPattern.CIRCLE)
        {

        }
	}
	
	// Update is called once per frame
	void Update () {
        
	}

	void FixedUpdate(){
        if (mPattern == PowerupPattern.LR){
            if (gameObject.transform.position.x < center.x - 3 || gameObject.transform.position.x > center.x + 3)
                gameObject.GetComponent<Rigidbody2D>().velocity = -gameObject.GetComponent<Rigidbody2D>().velocity;
        }
        else if (mPattern == PowerupPattern.UD)
        {
            if (gameObject.transform.position.y < center.y - 0.5 || gameObject.transform.position.y > center.y + 2.5)
                gameObject.GetComponent<Rigidbody2D>().velocity = -gameObject.GetComponent<Rigidbody2D>().velocity;
        }
        else if (mPattern == PowerupPattern.CIRCLE)
        {

        }
	}

    void OnTriggerEnter2D(Collider2D _collider){
        if (_collider.tag == "Player"){
            /*GameObject itemGenerator = GameObject.Find("ItemGenerator");
            GenerateItems igScript = itemGenerator.GetComponent<GenerateItems>();
            igScript.removeGameObject(gameObject);*/

            GameObject player = GameObject.Find(_collider.name);
            PlayerPowerups powerupScript = player.GetComponent<PlayerPowerups>();
            powerupScript.ActivatePowerUp(gameObject.tag);
        }
    }
    private PowerupPattern mPattern;
    private PowerupPattern[] mPatterns;
    private Vector2 center;
    private float origin;
}
