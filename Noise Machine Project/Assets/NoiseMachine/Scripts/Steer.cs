using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steer : MonoBehaviour
{
    public GameObject board;
    public GameObject player;

    public float gravity;
    // Update is called once per frame
    void FixedUpdate()
    {
        Turn();
        Fall();
    }

    void Turn()
    {
        if (Input.GetAxis("Horizontal") != 0)
        {
            //turns based on positive or negitive button press (right or left)
            board.transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 45 * ((Time.deltaTime * Input.GetAxis("Horizontal")) * 50), transform.localEulerAngles.z);
        }
    }

    void Fall()
    {
        //falls down the mountain in the direction the board is facing
        player.transform.position += transform.forward * (Time.deltaTime * gravity);
    }

}
