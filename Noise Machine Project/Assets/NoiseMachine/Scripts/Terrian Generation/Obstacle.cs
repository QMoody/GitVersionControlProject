using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public bool touchingGround = false;

    private void OnCollisionEnter(Collision other)
    {
        if (!touchingGround)
        { 
            //Check to see if the Collider's tag is "Ground"
            if (other.gameObject.tag == "Ground")
            {
                touchingGround = true;
            }

            if (!touchingGround)
            {
                //Output the message
                //Debug.Log(this.gameObject.name + " was hit by " + other.gameObject.name, this.gameObject);
                Traker.inst.AddHit();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!touchingGround)
        {
            //Check to see if the Collider's tag is "Ground"
            if (other.gameObject.tag == "Ground")
            {
                touchingGround = true;
            }

            if (!touchingGround)
            {
                //Output the message
                //SendMessage("Touched",null,SendMessageOptions.DontRequireReceiver);
                SendMessage("Touched");
                //Debug.Log(this.gameObject.name + " was hit by " + other.gameObject.name, this.gameObject);
            }
        }

    }

}
