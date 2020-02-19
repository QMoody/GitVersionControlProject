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
    }

    void Update()
    {
        //if cords are different check for new chunk
        CheckForNewChunk();

        //get player loc
        //check area around player for spawned chunks
        //if there is chuck missing too close to player spawn chuck and delete farthest chunk
    }
    
    private void CheckForNewChunk()
    {
        playerChunkLoc = new Vector2(Mathf.Round(playerObject.transform.position.x / realChunkSize), Mathf.Round(playerObject.transform.position.z / realChunkSize));
        int pChunkX = Mathf.RoundToInt(playerChunkLoc.x);
        int pChunkZ = Mathf.RoundToInt(playerChunkLoc.y);
        Debug.Log(playerChunkLoc);

        //Check if current area has chunk spawned
        bool isChunkSpawned = false;
        for (int i = 0; i < chunkPosAry.Length; i++)
            if (chunkPosAry[i].x == pChunkX && chunkPosAry[i].y == pChunkZ)
            {
                isChunkSpawned = true;
                break;
            }

        if (isChunkSpawned == false)
            CreateChunk(pChunkX, pChunkZ);
    }

    private void CreateChunk(int x, int z)
    {
        //Create new chunk
        GameObject chunk = Instantiate(noiseObject, new Vector3(-10 + x * realChunkSize, 0, -10 + z * realChunkSize), Quaternion.Euler(0,0,0));

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

        //chuckSpawned[x, z] = true;

        //make sure to mark position for where the current chunks are spawned

        //if chunck value over limit delete farthest chunks

        //if (AChunk.Length >= chunkLimit + 1)
        //{
        //
        //}
    }

    private void CheckForFarthestChunk()
    {

    }
}
