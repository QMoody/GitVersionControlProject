using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineSpawner : MonoBehaviour
{
    public BezierSpline spline;
    public GameObject StartPoint;
    public Transform leftMarker;
    public Transform rightMarker;
    public float xSpacing;
    public float zSpacing;
    private float left;
    private float right;

    private void Start()
    {
        for(int p = 0; p < spline.ControlPointCount; p+=3)
        {
            DropControlPoint(p);
        }
        for (int p = 1; p < spline.ControlPointCount; p += 3)
        {
            DropControlPoint(p);
        }
    }

    void DropControlPoint(int p)
    {
        float randomXaxis = PublicFunction.RoundUp(Random.Range(left, right), xSpacing); //The control point will only appear at a point that's a mupltable of the givin number 
        Vector3 randomPoint = new Vector3(randomXaxis, 0, p*zSpacing);

        RaycastHit hit;
        if (Physics.Raycast(randomPoint, Vector3.down, out hit, 2000)) //Cast a Raycast to see if any colliders are under that point.
        {
            spline.SetControlPoint(p,randomPoint + Vector3.down * hit.distance); //Drops point right on the surrface of that collider
        }
        else
        {
            spline.SetControlPoint(p, spline.GetControlPoint(p - 1));
        }
    }
}
