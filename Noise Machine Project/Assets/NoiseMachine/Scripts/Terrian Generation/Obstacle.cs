using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public bool touchingGround = false;
    public bool tested = false;

    private void OnCollisionStay(Collision other)
    {
        if (!tested)
        { 
            //Check to see if the Collider's tag is "Ground"
            if (other.gameObject.tag == "Ground")
            {
                touchingGround = true;
            }

            tested = true;

            if (!touchingGround)
            {
                //Output the message
                Debug.Log(this.gameObject.name + " is not touching ground", this.gameObject);
            }
        }

    }

}
