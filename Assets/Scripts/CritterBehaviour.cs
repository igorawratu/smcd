using UnityEngine;
using System.Collections;

public class CritterBehaviour : MonoBehaviour
{
    public float maxIdleTime = 10.0f;
    public float minIdleTime = 1.0f;
    public float runTime = 1.5f;
    public float moveSpeed = 1.5f;

    public GameObject SplatEffect;
    public Animator animator;
    private int running = Animator.StringToHash("running");

    private Transform graphic;

    private bool isRunning = false;
    public bool Running
    {
        get { return isRunning; } 
        set
        {
            isRunning = value;
            animator.SetBool(running, value);
        } 
    }

    private int direction = 1;
    public int Direction
    {
        get { return direction; }
        set { direction = value; }
    }

    private void Start()
    {
        GetComponent<BoxCollider2D>().enabled = true;
        graphic = GetComponentInChildren<Transform>();
        animator = GetComponent<Animator>();
        StartCoroutine(Idle());
    }

    private IEnumerator Idle()
    {

        while (enabled)
        {
            var idleTime = Random.Range(minIdleTime, maxIdleTime);
            yield return new WaitForSeconds(idleTime);
            yield return StartCoroutine(Run());
        }
    }

    private IEnumerator Run()
    {
        Running = true;
        var randomDirection = Random.value;
        if (randomDirection < 0.5f)
            Direction = 1; //multiplier for direction
        else
            Direction = -1;
        
        graphic.localScale = new Vector3(Direction, 1, 1); //set graphic facing direction
        
        var counter = 0.0f;
        while (counter < runTime)
        {
            counter += Time.deltaTime;
            transform.Translate(Vector2.right * Direction * moveSpeed * Time.deltaTime);
            yield return null;
        }
        Running = false;
        yield return true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //if (other.GetComponent<Rigidbody2D>().velocity.y < 0)
        //    return;

        GameObject splatEffect = (GameObject)Instantiate(SplatEffect, transform.position, Quaternion.identity);
        splatEffect.SetActive(true);
        LevelSounds.inst.playBreakableObject(transform.position);

        GameObject itemGenerator = GameObject.Find("ItemGenerator");
        GenerateItems igScript = itemGenerator.GetComponent<GenerateItems>();
        igScript.removeGameObject(gameObject);
        //Destroy(this.gameObject);
        
    }

    
}
