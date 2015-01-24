using UnityEngine;
using System.Collections;

public class PlayerCollision : MonoBehaviour
{
    void Start()
    {
        //Debug.Log(gameObject.tag);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //foreach (ContactPoint contact in collision.contacts)
        //{
        //    if(contact)
        //    Debug.DrawRay(contact.point, contact.normal, Color.white);
        //}
        
        if (collision.collider.gameObject.tag == "obstacle")
        {
            RandomShake.randomShake.PlaySinShake();
        }
    }
}