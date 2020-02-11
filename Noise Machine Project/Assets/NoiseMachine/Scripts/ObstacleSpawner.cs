﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject treePrefab;
    public GameObject player;
    private List<GameObject> treePrefabs;
    public float left;
    public float right;
    public Vector3 offsetFromPlayer;

    private void Start()
    {
        treePrefabs = new List<GameObject>(0);
    }

    void SpawnObstacle()
    {
        transform.position = new Vector3(Random.Range(left, right), transform.position.y, transform.position.z);

        RaycastHit hit;        
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 2000)) //Cast a Raycast to see if any colliders are under that point.
        {
           treePrefabs.Add(Instantiate(treePrefab, transform.position+transform.TransformDirection(Vector3.down)*hit.distance, Quaternion.identity)); //Instantiate an obstacle right on the surrface of that collider and add them to the list
        }
        
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            SpawnObstacle();
        }
        transform.position = new Vector3(transform.position.x, player.transform.position.y + offsetFromPlayer.y, player.transform.position.z + offsetFromPlayer.z);

        for(int t = 0; t < treePrefabs.Count; t++) // for every tree in the list
        {
            float distanceAway = (transform.position.z - treePrefabs[t].transform.position.z ); //calculate the distance way
            if (distanceAway> offsetFromPlayer.z * 1.5) // if it's far behind the player
            {
                Destroy(treePrefabs[t]); //DESTROY IT
                treePrefabs.Remove(treePrefabs[t]); //and remove the null refrence.
            }
        }
    }
}
