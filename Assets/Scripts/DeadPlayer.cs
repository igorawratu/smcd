using UnityEngine;
using System.Collections;

public class DeadPlayer : MonoBehaviour {
    private string mPKey;
    public LayerMask mask;

	// Use this for initialization
    void Awake()
    {
        mPKey = "a";
        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
        sr.color = new Color(255, 0, 0);
        Debug.Log("Start called");
    }

	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate(){
        if (gameObject.transform.position.y < 0)
        {
            gameObject.transform.position = new Vector2(gameObject.transform.position.x, 0);
            gameObject.rigidbody2D.velocity = new Vector2(gameObject.rigidbody2D.velocity.x, 0);
        }

        if (Input.GetKey(mPKey)){
            Vector2 position = (Vector2)gameObject.transform.position;
            float raycastLength = 0.8f;
            Vector2 down = -Vector2.up * raycastLength;
            
            bool onTheGround = Physics2D.Raycast(position, down, raycastLength, ~mask.value);
            Debug.Log(onTheGround ? "true" : "false");

            if(onTheGround)
                gameObject.rigidbody2D.velocity = new Vector2(gameObject.rigidbody2D.velocity.x, 25);
        }
    }

    public void setInfo(string _key, Color _col){
        mPKey = _key;
        gameObject.name = "dead" + mPKey;
        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
        sr.color = _col;
        Debug.Log("Setinfo called");
    }
}
