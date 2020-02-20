using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Traker : MonoBehaviour
{
    // Start is called before the first frame update

    float YSPos;
    float ZSPos;

    float YCPos;
    float ZCPos;

    public Text distance;

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

        distance.text = (YSPos-YCPos).ToString();

        if (YSPos - YCPos > 250)
        {
            print("You Win");
        }
    }

    float GetDistance()
    {
        return YCPos;
    }
}
