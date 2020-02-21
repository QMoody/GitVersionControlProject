using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGen : MonoBehaviour
{
    public GameObject playerObject;
    public GameObject noiseObject;
    public GameObject[] chunkAry;
    public Vector2[] chunkPosAry;
    public float chunckSpace;
    public int chunkLimit; //25 to start?

    public float chunkScale;
    public int realChunkSize;
    private Vector2 playerChunkLoc;
    private Vector2 playerChunkLoc_;
    
    void Start()
    {
        chunkAry = new GameObject[chunkLimit];
        chunkPosAry = new Vector2[chunkLimit];

        //Set chunkPosAry to non 0,0 values so creating new chunks can work properly
        for (int i = 0; i < chunkPosAry.Length; i++)
            chunkPosAry[i] = new Vector2(1000, 1000);

        playerChunkLoc_ = new Vector2(1000, 1000);

        CheckForNewChunk();
    }

    void Update()
    {
        CheckForNewChunk();
    }
    
    private void CheckForNewChunk()
    {
        playerChunkLoc = new Vector2(Mathf.Round(playerObject.transform.position.x / realChunkSize), Mathf.Round(playerObject.transform.position.z / realChunkSize));
        int pChunkX = Mathf.RoundToInt(playerChunkLoc.x);
        int pChunkZ = Mathf.RoundToInt(playerChunkLoc.y);
        //Debug.Log(playerChunkLoc);

        //if cords are different check if new area has chunk spawned
        if (playerChunkLoc_ != playerChunkLoc)
        {
            //Check if current area has chunk spawned
            bool[,] isChunkSpawned = new bool[3,3];
            for (int i = 0; i < chunkPosAry.Length; i++)
            {
                if (chunkPosAry[i].x == pChunkX && chunkPosAry[i].y == pChunkZ)
                    isChunkSpawned[1,1] = true;
                else if (chunkPosAry[i].x + 1 == pChunkX && chunkPosAry[i].y == pChunkZ)
                    isChunkSpawned[2,1] = true;
                else if (chunkPosAry[i].x - 1 == pChunkX && chunkPosAry[i].y == pChunkZ)
                    isChunkSpawned[0,1] = true;
                else if (chunkPosAry[i].x == pChunkX && chunkPosAry[i].y + 1 == pChunkZ)
                    isChunkSpawned[1,2] = true;
                else if (chunkPosAry[i].x == pChunkX && chunkPosAry[i].y - 1 == pChunkZ)
                    isChunkSpawned[1,0] = true;
                else if (chunkPosAry[i].x + 1 == pChunkX && chunkPosAry[i].y + 1 == pChunkZ)
                    isChunkSpawned[2,2] = true;
                else if (chunkPosAry[i].x + 1 == pChunkX && chunkPosAry[i].y - 1 == pChunkZ)
                    isChunkSpawned[2,0] = true;
                else if (chunkPosAry[i].x - 1 == pChunkX && chunkPosAry[i].y + 1 == pChunkZ)
                    isChunkSpawned[0,2] = true;
                else if (chunkPosAry[i].x - 1 == pChunkX && chunkPosAry[i].y - 1 == pChunkZ)
                    isChunkSpawned[0,0] = true;
            }

            for (int x = 0; x < isChunkSpawned.GetLength(0); x++)
                for (int z = 0; z < isChunkSpawned.GetLength(1); z++)
                    if (isChunkSpawned[x, z] == false)
                        CreateChunk(pChunkX, pChunkZ, x - 1, z - 1);

            //        |
            //+       Y
            // [2,2][1,2][0,2]
            // [2,1][1,1][0,1] X -
            // [2,0][1,0][0,0]
            //                -

            playerChunkLoc_ = playerChunkLoc;
        }
    }

    private void CreateChunk(int x, int z, int xAdd, int zAdd)
    {
        Debug.Log(xAdd + " / " + zAdd);
        //Create new chunk
        GameObject chunk = Instantiate(noiseObject, new Vector3(-10 + (x + xAdd) * realChunkSize, 0, -10 + (z + zAdd) * realChunkSize), Quaternion.Euler(0,0,0));

        //Check for free chunk array slot
        bool chunkIsFree = false;
        for (int i = 0; i < chunkAry.Length; i++)
        {
            if (chunkAry[i] == null)
            {
                chunkAry[i] = chunk;
                chunkPosAry[i] = new Vector2(x, z);
                chunkIsFree = true;
                break;
            }
        }

        //Find and delete farthest chunk from player if no free slot is open in the array
        if (chunkIsFree == false)
        {
            int farChunkId = 0;
            float farDis = 0;
            for (int i = 0; i < chunkAry.Length; i++)
                if (Vector3.Distance(chunkAry[i].transform.position, playerObject.transform.position) > farDis)
                {
                    farDis = Vector3.Distance(chunkAry[i].transform.position, playerObject.transform.position);
                    farChunkId = i;
                }

            Destroy(chunkAry[farChunkId]);
            chunkAry[farChunkId] = chunk;
            chunkPosAry[farChunkId] = new Vector2(x, z);
        }

        //Set rotation
        //Place in worled based on rotation
        //Set chunks to spawn when play is near it // Create moveable shere to test this
        //Get distance away from initial spawn maybe?

        //make sure to mark position for where the current chunks are spawned

        //if chunck value over limit delete farthest chunks
    }
}
