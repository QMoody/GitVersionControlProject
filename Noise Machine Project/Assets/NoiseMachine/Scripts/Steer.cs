using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steer : MonoBehaviour
{
    public GameObject board;
    // Update is called once per frame
    void FixedUpdate()
    {
        //testing with keyboard
       if (Input.GetAxis("Horizontal") != 0)
        {
            //turns based on positive or negitive button press (right or left)
            board.transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 45 * ((Time.deltaTime * Input.GetAxis("Horizontal")) * 50), transform.localEulerAngles.z);
        }
    }
}
