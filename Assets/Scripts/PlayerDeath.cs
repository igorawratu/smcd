using UnityEngine;
using System.Collections;

public class PlayerDeath : MonoBehaviour {

    public GameObject bloodEffect;
	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {


        Vector2 pos = transform.position;
        Vector2 cameraLeftPos = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
        if (pos.x < cameraLeftPos.x)
        {
            gameObject.SetActive(false);
            Instantiate(bloodEffect,
                new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, bloodEffect.transform.position.z),
                bloodEffect.transform.rotation);
            RandomShake.randomShake.PlayShakeX();

        }
	}
}
