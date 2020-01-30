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
<<<<<<< Updated upstream
       if (Input.GetAxis("Horizontal") != 0)
        {
            //turns based on positive or negitive button press (right or left)
            board.transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 45 * ((Time.deltaTime * Input.GetAxis("Horizontal")) * 50), transform.localEulerAngles.z);
        }
=======
       if (Input.GetAxis("Vertical") > 0)
        {
            //turns based on positive or negitive button press (right or left)
            player.transform.position += transform.forward * (Time.deltaTime * Input.GetAxis("Vertical") * 20);
        }
    }

    void Turn()
    {
            //turns based on mouse position on screen
            board.transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, (Mathf.Atan2(Input.mousePosition.x - Screen.width/2, Screen.height)*180/Mathf.PI), transform.localEulerAngles.z);
        
>>>>>>> Stashed changes
    }
}
