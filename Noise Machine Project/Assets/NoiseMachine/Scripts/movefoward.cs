using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movefoward : MonoBehaviour
{
    public float speed;
    public float acceleration;
    public float currentDistance;
    public float lastFrameDistance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.TransformDirection(Vector3.forward)*Time.deltaTime*speed;
        currentDistance = transform.position.z;
        if (lastFrameDistance < currentDistance)
            speed += acceleration * Time.deltaTime;
        else
            speed -= acceleration * Time.deltaTime * 2;
        lastFrameDistance = currentDistance;
    }
}
