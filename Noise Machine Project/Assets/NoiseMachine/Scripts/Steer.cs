using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steer : MonoBehaviour
{
    public GameObject board;
    public GameObject player;

    public float maxSpeed;
    private Rigidbody m_rb;
    private Vector3 angle;

    private void Start()
    {
        m_rb = transform.parent.GetComponent<Rigidbody>();
        angle = new Vector3();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Turn();
        Fall();
        player.transform.localEulerAngles = angle;
    }

    private void Update()
    {
        if (Input.GetAxis("Fire1")>=.01)
        {
            //Break();
        }
    }

    void Turn()
    {
        if (player.transform.eulerAngles.x > 0 ) //stops player from riding up the hill
        {
            //turns based on mouse position on screen
           angle = new Vector3(board.transform.localEulerAngles.x, (Mathf.Atan2(Input.mousePosition.x - Screen.width / 2, Screen.height) * 180 / Mathf.PI), board.transform.localEulerAngles.z);
           
        }
    }

    private void Break()
    {
        m_rb.drag = 20;
        //Vector3 breakForce = -m_rb.velocity * Time.deltaTime;
        //m_rb.AddRelativeForce(breakForce*10, ForceMode.Impulse);
        //print(breakForce);
    }

    void Fall()
    {
        //falls down the mountain in the direction the board is facing
        //m_rb.AddForce(transform.forward * (Time.deltaTime * (player.transform.localEulerAngles.x/90) * maxSpeed),ForceMode.Impulse);
        //print(player.transform.localEulerAngles.x / 90); //testing gravity scaling (simulates picking up speed)
    }
    
}
