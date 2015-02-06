using UnityEngine;
using System.Collections;

public class FinishedParticleRemover : MonoBehaviour
{
    public float deathTime = 0.0f;

	// Use this for initialization
	void Start () 
    {
        Invoke("destroyParticle", deathTime);
	}

    void destroyParticle()
    {
        Destroy(gameObject);
    }
    //// Update is called once per frame
    //void Update () 
    //{
    //}
}
