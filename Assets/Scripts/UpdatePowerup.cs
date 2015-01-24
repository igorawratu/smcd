using UnityEngine;
using System.Collections;

public class UpdatePowerup : MonoBehaviour {

	// Use this for initialization
	void Start () {
        mDeactivated = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate(){
        
	}

    void OnTriggerEnter(Collider _collider){
        if (_collider.tag == "someplayer" && !mDeactivated){
            GameObject itemGenerator = GameObject.Find("ItemGenerator");
            GenerateItems igScript = itemGenerator.GetComponent<GenerateItems>();
            igScript.removeGameObject(gameObject);
            GameObject player = GameObject.Find(_collider.name);
            PlayerMovement pm = player.GetComponent<PlayerMovement>();
            pm.ActivatePowerUp(gameObject.tag);
            mDeactivated = true;
        }
    }

    private bool mDeactivated;
}
