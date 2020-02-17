using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineSpawner : MonoBehaviour
{
    public BezierSpline spline;
    public GameObject startPoint;
    public Transform leftMarker;
    public Transform rightMarker;
    public float xSpacing;
    public float zSpacing;
    private float left;
    private float right;

    private void Start()
    {
        left = leftMarker.position.x;
        right = rightMarker.position.x;
        spline.SetControlPoint(0, DropPoint(startPoint.transform.position));
        for (int p = 3; p < spline.ControlPointCount; p+=3)
        {                 
            spline.SetControlPoint(p, DropPoint(randomPoint(p))); //Drops point right on the surrface of that collider
        }
        for (int p = 1; p < spline.ControlPointCount; p += 3)
        {
            spline.SetControlPoint(p, DropPoint(randomPoint(p))); //Drops point right on the surrface of that collider
        }
        for (int p = 2; p < spline.ControlPointCount; p += 3)
        {
            spline.SetControlPoint(p, DropPoint(randomPoint(p)));
        }
    }

    Vector3 randomPoint(int p)
    {
        float randomXaxis = PublicFunction.RoundUp(Random.Range(left, right), xSpacing); //The control point will only appear at a point that's a mupltable of the givin number 
        return new Vector3(randomXaxis, 0, p * zSpacing);
    }

    Vector3 DropPoint(Vector3 point)
    {      

        RaycastHit hit;
        if (Physics.Raycast(point, Vector3.down, out hit, 2000)) //Cast a Raycast to see if any colliders are under that point.
        {
            return point + Vector3.down * hit.distance;            
        }
        else
        {
            return point;
        }
    }
}
