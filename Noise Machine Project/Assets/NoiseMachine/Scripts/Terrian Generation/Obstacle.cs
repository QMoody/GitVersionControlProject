using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public bool touchingGround = false;

    private void OnTriggerStay(Collider other)
    {
        //Check to see if the Collider's tag is "Ground"
        if (other.tag == "Ground")
        {
            touchingGround = true;
        }
    }
    

    void Update()
    {
        if (!touchingGround)
        {
            //Output the message
            Debug.Log(this.gameObject.name + " is not touching ground",this.gameObject);
        }
    }
}
