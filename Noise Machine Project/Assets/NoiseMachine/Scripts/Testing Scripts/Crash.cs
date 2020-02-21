using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crash : MonoBehaviour
{
    private Steer steer;
    // Start is called before the first frame update
    void Start()
    {
        steer = GetComponent<Steer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Obstacle")
            steer.enabled = false;
    }
}
