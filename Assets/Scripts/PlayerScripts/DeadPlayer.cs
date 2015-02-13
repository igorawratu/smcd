using UnityEngine;
using System.Collections;

public class DeadPlayer : MonoBehaviour {
    private KeyCode mPKey;
    public LayerMask mask;
    public Font font;
    public float lowGravityScale;
    public float normalGravityScale;
    public float inverseGravityVel;

	// Use this for initialization
    void Awake()
    {
        mPKey = KeyCode.A;
        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
        sr.color = new Color(255, 0, 0);
        mLevel = LevelTypeManager.currentLevel;

        if(mLevel == LevelTypeManager.Level.lowGravity)
            rigidbody2D.gravityScale = lowGravityScale;
        else if(mLevel == LevelTypeManager.Level.gravityFlip)
            rigidbody2D.gravityScale = 0;
        else rigidbody2D.gravityScale = normalGravityScale;

        inverseGravityVel = -10;
        gameObject.rigidbody2D.velocity = new Vector2(gameObject.rigidbody2D.velocity.x, -10);
    }

	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate(){
        Debug.Log(gameObject.rigidbody2D.velocity.y);
        if (gameObject.transform.position.y < 0)
        {
            gameObject.transform.position = new Vector2(gameObject.transform.position.x, 0);
            gameObject.rigidbody2D.velocity = new Vector2(gameObject.rigidbody2D.velocity.x, 0);
        }

        if(mLevel == LevelTypeManager.Level.standard || mLevel == LevelTypeManager.Level.lowGravity)
            jump();
        else if(mLevel == LevelTypeManager.Level.gravityFlip) {
            invertGravity();
        }
        else if(mLevel == LevelTypeManager.Level.flappyBird)
            flap();
    }

    public void setInfo(KeyCode _key, Color _col){
        mPKey = _key;
        gameObject.name = "dead" + mPKey;
        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
        sr.color = _col;

        GameObject textChild = transform.FindChild("DeadPlayerText").gameObject;
        DeadPlayerText dptScript = textChild.GetComponent<DeadPlayerText>();
        dptScript.setName(mPKey.ToString());
    }

    public void getInfo(out KeyCode _key, out Color _col) {
        _key = mPKey;
        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
        _col = sr.color;
    }

    private void jump() {
        if(Input.GetKey(mPKey)) {
            Vector2 position = (Vector2)gameObject.transform.position;
            float raycastLength = 0.9f;
            Vector2 down = -Vector2.up * raycastLength;

            bool onTheGround = Physics2D.Raycast(position, down, raycastLength, ~mask.value);
            if(onTheGround)
                gameObject.rigidbody2D.velocity = new Vector2(gameObject.rigidbody2D.velocity.x, 20);
        }
    }

    private void flap() {
        Vector3 worldPos = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));
        if(Input.GetKey(mPKey) && gameObject.transform.position.y < worldPos.y - 0.5f) {
            gameObject.rigidbody2D.velocity = new Vector2(gameObject.rigidbody2D.velocity.x, 20);

            if(gameObject.transform.position.y > worldPos.y) {
                gameObject.transform.position = new Vector2(gameObject.transform.position.x, worldPos.y);
                gameObject.rigidbody2D.velocity = new Vector2(gameObject.rigidbody2D.velocity.x, 0);
            }
        }
    }

    void invertGravity() {
        Vector3 worldPos = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));

        if(Input.GetKey(mPKey)) {
            
            Vector2 position = (Vector2)gameObject.transform.position;
            float raycastLength = 0.9f;
            Vector2 cast = inverseGravityVel > 0 ? Vector2.up : -Vector2.up;
            cast *= raycastLength;

            bool canjump = Physics2D.Raycast(position, cast, raycastLength, ~mask.value);
            if(canjump) {
                inverseGravityVel *= -1;
                gameObject.rigidbody2D.velocity = new Vector2(gameObject.rigidbody2D.velocity.x, inverseGravityVel);
            }
        }
    }

    private LevelTypeManager.Level mLevel;
}
