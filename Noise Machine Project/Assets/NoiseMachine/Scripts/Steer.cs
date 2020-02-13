using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steer : MonoBehaviour
{
    public GameObject board;
    public GameObject player;

    public float maxSpeed;
    public Rigidbody m_rb;

    private void Start()
    {
        m_rb = transform.parent.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Turn();
        Fall();
    }

    void Turn()
    {
        if (player.transform.localEulerAngles.x > 0) //stops player from riding up the hill
        {
            //turns based on mouse position on screen
            board.transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, (Mathf.Atan2(Input.mousePosition.x - Screen.width / 2, Screen.height) * 180 / Mathf.PI), transform.localEulerAngles.z);
        }
    }

    void Fall()
    {
        //falls down the mountain in the direction the board is facing
        m_rb.AddForce(transform.forward * (Time.deltaTime * (player.transform.localEulerAngles.x/90) * maxSpeed),ForceMode.Impulse);
        //print(player.transform.localEulerAngles.x / 90); //testing gravity scaling (simulates picking up speed)
    }
    
}
