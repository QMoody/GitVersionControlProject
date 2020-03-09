using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGen : MonoBehaviour
{
    #region Variables
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

    public int chunkSpawnRadius; //Only odd numbers
    #endregion

    //--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//

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
        //Set location of player by chunk rather then real world cords
        playerChunkLoc = new Vector2(Mathf.Round(playerObject.transform.position.x / realChunkSize), Mathf.Round(playerObject.transform.position.z / realChunkSize));
        int pChunkX = Mathf.RoundToInt(playerChunkLoc.x);
        int pChunkZ = Mathf.RoundToInt(playerChunkLoc.y);

        //If cords are different check if new area has chunks already spawned
        if (playerChunkLoc_ != playerChunkLoc)
        {
            for (int x = 0; x < chunkSpawnRadius; x++)
                for (int z = 0; z < chunkSpawnRadius; z++)
                {
                    bool hasChunk = false;
                    for (int i = 0; i < chunkPosAry.Length; i++)
                        if (chunkPosAry[i] == new Vector2(x - 1 + pChunkX, z - 1 + pChunkZ))
                        {
                            hasChunk = true;
                            break;
                        }

                    if (hasChunk == false)
                        CreateChunk(x - 1 + pChunkX, z - 1 + pChunkZ, pChunkX, pChunkZ);
                }

            playerChunkLoc_ = playerChunkLoc;
        }
    }

    private void CreateChunk(int x, int z, int pChunkX, int pChunkZ)
    {
        //Create new chunk
        GameObject chunk = Instantiate(noiseObject, new Vector3(-realChunkSize / 2 + x * realChunkSize, 0, -realChunkSize / 2 + z * realChunkSize), Quaternion.Euler(0,0,0));

        //Check for free chunk array slot
        bool chunkIsFree = true;
        bool arrayIsNull = false;
        int freeChunkId = 0;
        for (int i = 0; i < chunkPosAry.Length; i++)
        {
            if (chunkAry[i] == null)
            {
                freeChunkId = i;
                arrayIsNull = true;
                break;
            }
        }

        //This needs to be changed to check for the farthest chunk away to destory
        //See unsed code a bottom of script
        //Check for which chunk can be replaced
        if (arrayIsNull == false)
            for (int i = 0; i < chunkPosAry.Length; i++)
            {
                for (int xi = 0; xi < chunkSpawnRadius; xi++)
                {
                    for (int zi = 0; zi < chunkSpawnRadius; zi++)
                    {
                        if (chunkPosAry[i] == new Vector2(xi - 1 + pChunkX, zi - 1 + pChunkZ))
                        {
                            chunkIsFree = false;
                            break;
                        }
                    }
                    if (chunkIsFree == false)
                        break;
                }

                if (chunkIsFree == true)
                {
                    freeChunkId = i;
                    Destroy(chunkAry[freeChunkId]);
                    break;
                }

                chunkIsFree = true;
            }

        //Add new chunk to array
        chunkAry[freeChunkId] = chunk;
        chunkPosAry[freeChunkId] = new Vector2(x, z);


        //Find and delete farthest chunk from player if no free slot is open in the array
        /*
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
        */

        //Set rotation
        //Place in worled based on rotation
        //Set chunks to spawn when play is near it // Create moveable shere to test this
        //Get distance away from initial spawn maybe?

        //make sure to mark position for where the current chunks are spawned

        //if chunck value over limit delete farthest chunks
    }
}
