using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGen : MonoBehaviour
{
    public GameObject noiseObject;
    public GameObject[] AChunk;
    public float chunckSpace;
    public int chunkLimit; //25 to start?

    void Start()
    {
        AChunk = new GameObject[chunkLimit + (chunkLimit / 4)];
    }

    void Update()
    {
        
    }

    private void CreateChunk()
    {
        //GameObject chunk = Instantiate(noiseObject, new Vector3(,,), Quaternion.Euler(,,));
        //Set rotation
        //Place in worled based on rotation
        //Set chunks to spawn when play is near it // Create moveable shere to test this
        //Get distance away from initial spawn maybe?

        //make sure to mark position for where the current chunks are spawned

        //if chunck value over limit delete farthest chunks
    }
}
