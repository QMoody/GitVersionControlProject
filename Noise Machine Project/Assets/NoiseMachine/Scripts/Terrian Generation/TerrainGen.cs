using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
    public bool isInArea;
    public GameObject chunkObj;
}

public class TerrainGen : MonoBehaviour
{
    #region Variables
    public GameObject playerObject;
    public GameObject noiseObject;
    public Material planeTexture;
    public PhysicMaterial physicsMat;
    public float chunckSpace;
    public int chunkLimit;
    public bool flipTerrainSlope;

    Dictionary<Vector2, Chunk> chunks = new Dictionary<Vector2, Chunk>();

    public float chunkScale;
    public int chunkSize; //Numbers that can be divided by 2 and preferably over 20
    private int realChunkSize;
    [Range(0.0f, 10.0f)] public float perlinFreq;

    public Vector2 playerChunkLoc;
    public Vector2 playerChunkLoc_;
    public bool isFirstSpawn = true;
    private float terrainFlipValue;

    public int chunkSpawnRadius; //Only odd numbers
    #endregion

    //--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//

    void Start()
    {
        isFirstSpawn = true;

        if (flipTerrainSlope == true)
            terrainFlipValue = -1f;
        else
            terrainFlipValue = 1f;

        realChunkSize = (int)(chunkSize * chunkScale);
        chunkLimit = chunkSpawnRadius * chunkSpawnRadius;

        playerChunkLoc_ = new Vector2(1000, 1000);

        for (int i = 0; i < chunkLimit; i++)
            CreateChunks(i);

        CheckForNewChunk();

        isFirstSpawn = false;
    }

    void Update()
    {
        CheckForNewChunk();
    }

    private void CheckForNewChunk()
    {
        //if no object exsits 

        //Set location of player by chunk rather then real world cords
        playerChunkLoc = new Vector2(Mathf.Round(playerObject.transform.position.x / realChunkSize), Mathf.Round(playerObject.transform.position.z / realChunkSize));
        int pChunkX = Mathf.RoundToInt(playerChunkLoc.x);
        int pChunkZ = Mathf.RoundToInt(playerChunkLoc.y);

        int radiusStart = -(chunkSpawnRadius - 1) / 2;
        int radiusEnd = (chunkSpawnRadius - 1) / 2;

        Chunk tmpChunk = null;
        int il = 0;
        Vector2[] newChunkLocAry = new Vector2[chunkLimit];
        if (playerChunkLoc_ != playerChunkLoc)
        {
            foreach (KeyValuePair<Vector2, Chunk> entry in chunks)
            {
                chunks[entry.Key].isInArea = false;
            }

            for (int x = 0; x < chunkSpawnRadius; x++)
                for (int z = 0; z < chunkSpawnRadius; z++)
                {
                    if (chunks.TryGetValue(new Vector2(radiusStart + x + pChunkX, radiusStart + z + pChunkZ), out tmpChunk))
                    {
                        tmpChunk.isInArea = true;
                    }
                    else
                    {
                        newChunkLocAry[il] = new Vector2(x, z);
                        il++;
                    }

                }

            Vector2[] keyList = new Vector2[il];
            for (int i = 0; i < il; i++)
            {
                Debug.Log("Loc");

                Vector2 tmpKey = new Vector2(100, 100);
                foreach (KeyValuePair<Vector2, Chunk> entry in chunks)
                {
                    if (chunks[entry.Key].isInArea == false)
                    {
                        tmpKey = entry.Key;
                        break;
                    }
                }
                UpdateChunk(tmpKey, (int)newChunkLocAry[i].x + pChunkX + radiusStart, (int)newChunkLocAry[i].y + pChunkZ + radiusStart, pChunkX, pChunkZ);
            }

            //for (int i = 0; i < il; i++)
            //    UpdateChunk(entry.Key, (int)Loc.x + pChunkX + radiusStart, (int)Loc.y + pChunkZ + radiusStart, pChunkX, pChunkZ);

            playerChunkLoc_ = playerChunkLoc;
        }
    }

    private void UpdateChunk(Vector2 key, int x, int z, int pChunkX, int pChunkZ)
    {
        Debug.Log("UpdateC");
        //Create new chunk
        Vector2 setLoc = new Vector2(-realChunkSize / 2 + x * realChunkSize, -realChunkSize / 2 + z * realChunkSize);
        chunks[key].chunkObj.transform.position = new Vector3(setLoc.x, terrainFlipValue * z * chunkScale * 16, setLoc.y);
        chunks[key].isInArea = true;
        Noise noise = chunks[key].chunkObj.GetComponent<Noise>();

        noise.planeWorldPos = new Vector2(x, z);
        //if (isFirstSpawn == false)
        //    noise.UpdateField();

        chunks[new Vector2(x, z)] = chunks[key];
        chunks.Remove(key);

        if (isFirstSpawn == false)
            noise.UpdateField();
    }

    void CreateChunks(int i)
    {
        GameObject chunk = Instantiate(noiseObject);

        Noise noise = chunk.GetComponent<Noise>();

        noise.planeSize = chunkSize + 1;
        noise.planeScale = chunkScale;
        noise.perlinFreq = perlinFreq;
        noise.textureMat = planeTexture;
        noise.physMat = physicsMat;
        noise.chunkFlipValue = terrainFlipValue;

        Chunk c = new Chunk();
        c.chunkObj = chunk;
        chunks.Add(new Vector2(1000 + i, 1000 + i), c);
    }
}
