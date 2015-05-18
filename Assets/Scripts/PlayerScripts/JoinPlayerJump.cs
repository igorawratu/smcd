using UnityEngine;
using System.Collections;

public class JoinPlayerJump : MonoBehaviour {
    private KeyCode mPKey;

    public LayerMask mask;
    public float jumpVel = 2.0f;

    public float gravScale = 8.0f;

    Vector2 vel = Vector2.zero;
    public float jumpDelay = 0.3f;
    bool canJump = true;
    public Animator animator;
    public AnimationBoard animation;
   
    //public void playIdle()
    //{
    //    animator.SetTrigger(idle);
    //}
    // Use this for initialization
    void Awake()
    {
        mPKey = KeyCode.A;
        vel = Vector2.zero;
        animation = GetComponentInChildren<AnimationBoard>();

    }
	// Use this for initialization
	void Start () 
    {
        animation.IntroMode = true;
        animation.FlappyMode = false;
	}
	
    //// Update is called once per frame
    //void Update () {
    bool calledFalling = false;
    bool onTheGroundLast = false;
    //}
    void FixedUpdate()
    {
        //Debug.Log(animator.GetCurrentAnimatorStateInfo(0).IsName("idle"));
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("intro"))
        {
            Vector2 position = (Vector2)gameObject.transform.position;
            float raycastLength = 0.5f;
            Vector2 down = -Vector2.up * raycastLength;
            bool onTheGround = Physics2D.Raycast(position, down, raycastLength, ~mask.value);
            Debug.DrawRay(position, down,Color.red, raycastLength);
            vel += Physics2D.gravity * gravScale * Time.deltaTime;

            if (onTheGround)
            {
                if (!onTheGroundLast)
                {
                    animation.Land();
                    calledFalling = false;
                }

                vel = new Vector2(0, 0);
                if (Input.GetKey(mPKey) && canJump)
                {
                    vel = new Vector2(0, jumpVel);
                    animation.Jump();
                    calledFalling = false;
                    canJump = false;
                    //PowerupSounds.inst.playDoubleJump();
                    SoundManager.instance.playMenuJump(transform.position);
                    Invoke("setCanJump", jumpDelay);
                }
            }
            else
            {
                if (!calledFalling && vel.y < 0)
                {
                    animation.Fall();
                    calledFalling = true;
                }
            }

            gameObject.transform.position = position + vel*Time.deltaTime;

            if (gameObject.transform.position.y < 1)
            {
                gameObject.transform.position = new Vector2(gameObject.transform.position.x, 1);
            }
            onTheGroundLast = onTheGround;
        }
        
    }
    public void setCanJump()
    {
        canJump = true;
    }
    public void setKey(KeyCode _key)
    {
        mPKey = _key;
    }
}
