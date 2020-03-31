using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineSpawner : MonoBehaviour
{
    public BezierSpline spline;
    public GameObject colliderPrefab;
    public List<GameObject> colliders;
    public GameObject startPoint;
    public Transform leftMarker;
    public Transform rightMarker;
    public float colliderSize;
    private float pathLenght;
    private int colliderCount;
    public float xSpacing;
    public float zSpacing;
    private float left;
    private float right;
    public float frequency;

    private void Awake()
    {
        left = leftMarker.position.x;
        right = rightMarker.position.x;
        spline.SetControlPoint(0, DropPoint(startPoint.transform.position));
        for (int p = 3; p < spline.ControlPointCount; p += 3)
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
        SpawnInvisableColliders();
    }

    private void SpawnInvisableColliders()
    {
        if (colliderSize == 0)
        {
            colliderSize = colliderPrefab.transform.localScale.x;
        }
        else
        {
            colliderPrefab.transform.localScale = new Vector3(colliderSize, colliderSize, colliderSize);
        }
        pathLenght = zSpacing * spline.ControlPointCount;
        colliderCount = (int)(pathLenght / colliderSize / 2);

        for (int p = 0; p < colliderCount; p++)
            colliders.Add(colliderPrefab);

        if (frequency <= 0 || colliders == null || colliders.Count == 0)
        {
            return;
        }
        float stepSize = 1f / (frequency * colliders.Count);
        for (int p = 0, f = 0; f < frequency; f++)
        {
            for (int i = 0; i < colliders.Count; i++, p++)
            {
                Transform item = Instantiate(colliders[i].transform) as Transform;
                item.gameObject.name = "NoSpawner " + p.ToString();
                Vector3 position = spline.GetPoint(p * stepSize);
                item.transform.localPosition = position;
                item.transform.parent = spline.gameObject.transform;
            }
        }
    }

    Vector3 randomPoint(int p)
    {
        float randomXaxis = PublicFunction.RoundUp(UnityEngine.Random.Range(left, right), xSpacing); //The control point will only appear at a point that's a mupltable of the givin number 
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
