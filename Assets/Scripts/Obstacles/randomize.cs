using UnityEngine;
using System.Collections;

public class randomize : MonoBehaviour
{

    public Sprite[] spriteVarients;
    private SpriteRenderer sr;

	void Awake ()
	{
	    sr = GetComponent<SpriteRenderer>();

	    var randomIndex = Random.Range(0, spriteVarients.Length);
	    sr.sprite = spriteVarients[randomIndex];
	}

    public void flip() {
        gameObject.transform.Rotate(new Vector3(180, 0, 0));
    }
	
}
