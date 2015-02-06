using UnityEngine;
using System.Collections;

public class PlayerCollision : MonoBehaviour
{
    public float raycastLength = 0.4f;
    void Start()
    {
        //Debug.Log(gameObject.tag);
    }
    //void Update()
    //{
    //    Vector2 right = -Vector2.right * raycastLength;
    //    Debug.DrawRay(transform.position, right, Color.green);
    //    RaycastHit2D hit = Physics2D.Raycast(position, right, raycastLength, ~mask.value);

    //    bool infront = true;
    //    if (hit.collider == null)
    //    {
    //        onTheGround = false;
    //    }
    //}
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