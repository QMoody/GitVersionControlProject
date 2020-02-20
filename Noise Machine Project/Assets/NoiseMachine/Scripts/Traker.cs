using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traker : MonoBehaviour
{
    // Start is called before the first frame update

    float YSPos;
    float ZSPos;

    float YCPos;
    float ZCPos;

    void Start()
    {
        YSPos = transform.position.y;
        ZSPos = transform.position.z;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        YCPos = transform.position.y;
        ZCPos = transform.position.z;
    }

    float GetDistance()
    {
        return YCPos;
    }
}
