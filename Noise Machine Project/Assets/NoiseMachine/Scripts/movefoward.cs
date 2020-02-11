using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movefoward : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.TransformDirection(Vector3.forward)*Time.deltaTime;
    }
}
