using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGen : MonoBehaviour
{
    public GameObject playerObject;
    public GameObject noiseObject;
    public GameObject[] AChunk;
    public bool[,] chuckSpawned;
    public float chunckSpace;
    public int chunkLimit; //25 to start?

    public int realChunkX; //20
    public int realChunkZ; //20
    private Vector2 playerRealChunkLoc;
    private Vector2 playerIntChunkLoc;

    void Start()
    {
        AChunk = new GameObject[chunkLimit];
    }

    void Update()
    {
        CheckForNewChunk();

        //get player loc
        //check area around player for spawned chunks
        //if there is chuck missing too close to player spawn chuck and delete farthest chunk
    }

    private void CheckForNewChunk()
    {
        playerRealChunkLoc = new Vector2(playerObject.transform.position.x, playerObject.transform.position.z);

        //round to nearest 20


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

        if (AChunk.Length >= chunkLimit + 1)
        {

        }
    }

    private void CheckForFarthestChunk()
    {

    }
}
