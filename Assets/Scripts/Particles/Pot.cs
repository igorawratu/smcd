using UnityEngine;
using System.Collections;

public class Pot : MonoBehaviour
{

    public GameObject PotSmashEffect;
    private SpriteRenderer sr;


    public void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.enabled = true;
        PotSmashEffect.SetActive(false);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player") {
            GameObject player = GameObject.Find(other.name);
            PlayerMovement movscript = player.GetComponent<PlayerMovement>();
            movscript.incPots();
        }
        
        PotSmashEffect.SetActive(true);
        sr.enabled = false;
        LevelSounds.inst.playBreakableObject(transform.position);
    }


}
