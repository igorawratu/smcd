using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CloudMovement : MonoBehaviour {

    public Vector2 speedRange = new Vector2(0.6f, 1.0f);
	// Use this for initialization
    public List<Sprite> sprites = new List<Sprite>();

    float speed = 0;
	void Start () 
    {
        int r = Random.Range(0,3);
        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
        sr.sprite = sprites[r];
        speed = Random.Range(speedRange.x, speedRange.y);
        //Vector3 pos = gameObject.transform.position;
        //pos.y += Random.Range(-1.0f, 1.0f);
        //gameObject.transform.position = pos;

	}
    


    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 moveVel = Vector3.zero;
        moveVel.x = VariableSpeed.currentCloudSpeed * speed;
        Vector3 currentPos = gameObject.transform.position;
        currentPos += moveVel * Time.deltaTime;

        gameObject.transform.position = currentPos;
    }
}
