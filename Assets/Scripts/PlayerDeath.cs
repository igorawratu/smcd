using UnityEngine;
using System.Collections;

public class PlayerDeath : MonoBehaviour {

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
            GameObject itemGenerator = GameObject.Find("WinnerChecker");
            WinnerChecker script = itemGenerator.GetComponent<WinnerChecker>();
            script.removePlayer(gameObject.name);

            gameObject.SetActive(false);
        }
	}
}
