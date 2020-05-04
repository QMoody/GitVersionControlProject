using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barriers : MonoBehaviour
{
    public GameObject meshObject;
    float approximation;
    float maxDistance;
    Material material;
    Color color;
    // Start is called before the first frame update
    void Start()
    {
        material = meshObject.GetComponent<MeshRenderer>().material;
        color.a = 0;
        material.color = color;
        maxDistance = GetComponent<Collider>().bounds.size.x;
    }

    private void OnTriggerEnter(Collider other)
    {
        approximation = Mathf.Abs(other.transform.position.x - meshObject.transform.position.x);

        color.a = Mathf.Lerp(0, 1, approximation / maxDistance);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
