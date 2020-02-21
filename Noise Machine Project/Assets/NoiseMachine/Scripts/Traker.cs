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

    float YPOSroc;
    float ZPOSroc;

    float totalDis;
    float goalDis;

    float timeS;
    Vector3 lastPoint;

    float locDisStart;

    public GameObject distance;
    public GameObject UI;

    public GameObject speedometer;

    void Start()
    {
        YSPos = transform.position.y;
        ZSPos = transform.position.z;
        locDisStart = distance.GetComponent<RectTransform>().rect.y;

        goalDis = 250;
        timeS = Time.time;
        lastPoint = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        YCPos = transform.position.y;
        ZCPos = transform.position.z;

        YPOSroc = (YSPos - YCPos);
        ZPOSroc = (ZCPos - ZSPos);

        if (Time.time - timeS > 1)
        {

            speedometer.GetComponent<Text>().text = (transform.position - lastPoint)*Time.deltaTime * 60 * 60).ToString() + " km/h";

            timeS = Time.time;
            lastPoint = transform.position;
        }
        speedometer.GetComponent<Text>().text = (totalDis*Time.deltaTime).ToString();


        totalDis = Mathf.Sqrt((YPOSroc * YPOSroc) + (ZPOSroc * ZPOSroc));

        DistanceTravelled();

        distance.GetComponent<Text>().text = (totalDis).ToString();
        //distance.text = (ZPOSroc).ToString();

        if (totalDis > goalDis)
        {
            print("You Win");
        }
    }

    float GetDistance()
    {
        return YCPos;
    }

    void DistanceTravelled()
    {
        //Shows the elevation of the player moving towards the goal by moving the text displaying the elevation from the start until the goal (happens in the position.y)
        distance.GetComponent<RectTransform>().position = new Vector3(distance.GetComponent<RectTransform>().position.x, 500 - ( 500 * (totalDis / goalDis)), distance.GetComponent<RectTransform>().position.z);

    }
}
