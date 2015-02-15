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
        PotSmashEffect.SetActive(true);
        sr.enabled = false;
    }


}
