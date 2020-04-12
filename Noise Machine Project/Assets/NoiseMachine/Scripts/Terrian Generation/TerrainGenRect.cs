using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TerrainGenRect : MonoBehaviour
{
    #region Variables
    public GameObject playerObject;
    public GameObject noiseObject;
    public Material planeTexture;
    public PhysicMaterial physicsMat;
    public float chunckSpace;
    int chunkLimit;
    public bool flipTerrainSlope;

    Dictionary<Vector2, Chunk> chunks = new Dictionary<Vector2, Chunk>();

    public float chunkScale;
    public int chunkSize; //Numbers that can be divided by 2 and preferably over 20
    private int realChunkSize;
    [Range(0.0f, 10.0f)] public float perlinFreq;
    public float heightScale;
    public float terrainSlopeValue;

    public Vector2 playerChunkLoc;
    [HideInInspector]
    public Vector2 playerChunkLoc_;
    public bool isFirstSpawn = true;
    private float terrainFlipValue;

    public int chunkSpawnWidth;
    public int chunkSpawnDepth;//Only odd numbers
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
        chunkLimit = chunkSpawnDepth * chunkSpawnWidth;

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
    int chunkOffsetDepth;
    int chunkOffsetWidth;
    int posOffset;

    int pChunkX;
    int pChunkZ;

    int areaFront;
    int areaBack;
    int areaLeft;
    int areaRight;

    int il;
    Vector2[] newChunkLocAry;
    Vector2[] keyList;
    Vector2 tmpKey;
    Chunk tmpChunk;

    private void CheckForNewChunk()
    {
        chunkOffsetDepth = (int)((chunkSpawnDepth - 1) / 2 * -terrainFlipValue);
        chunkOffsetWidth = (int)((chunkSpawnWidth - 1) / 2 * -terrainFlipValue);
        posOffset = (int)(realChunkSize / 2 * -terrainFlipValue);

        //Set location of player by chunk rather then real world cords
        playerChunkLoc = new Vector2(Mathf.Round(playerObject.transform.position.x / realChunkSize), Mathf.Round((playerObject.transform.position.z - posOffset) / realChunkSize) + chunkOffsetDepth);
        pChunkX = Mathf.RoundToInt(playerChunkLoc.x);
        pChunkZ = Mathf.RoundToInt(playerChunkLoc.y);

        areaFront = -(chunkSpawnDepth - 1) / 2;
        areaBack = (chunkSpawnDepth - 1) / 2;
        areaLeft = -(chunkSpawnWidth - 1) / 2;
        areaRight = (chunkSpawnWidth - 1) / 2;

        tmpChunk = null;
        il = 0;
        newChunkLocAry = new Vector2[chunkLimit];
        if (playerChunkLoc_ != playerChunkLoc)
        {
            foreach (KeyValuePair<Vector2, Chunk> entry in chunks)
            {
                chunks[entry.Key].isInArea = false;
            }

            for (int x = 0; x < chunkSpawnWidth; x++)
                for (int z = 0; z < chunkSpawnDepth; z++)
                {
                    if (chunks.TryGetValue(new Vector2(areaLeft + x + pChunkX, areaFront + z + pChunkZ), out tmpChunk))
                    {
                        tmpChunk.isInArea = true;
                    }
                    else
                    {
                        newChunkLocAry[il] = new Vector2(x, z);
                        il++;
                    }

                }

            keyList = new Vector2[il];
            for (int i = 0; i < il; i++)
            {
                //Debug.Log("Loc");

                tmpKey = new Vector2(100, 100);
                foreach (KeyValuePair<Vector2, Chunk> entry in chunks)
                {
                    if (chunks[entry.Key].isInArea == false)
                    {
                        tmpKey = entry.Key;
                        break;
                    }
                }
                UpdateChunk(tmpKey, (int)newChunkLocAry[i].x + pChunkX + areaFront, (int)newChunkLocAry[i].y + pChunkZ + areaFront, pChunkX, pChunkZ);
            }

            //for (int i = 0; i < il; i++)
            //    UpdateChunk(entry.Key, (int)Loc.x + pChunkX + radiusStart, (int)Loc.y + pChunkZ + radiusStart, pChunkX, pChunkZ);

            playerChunkLoc_ = playerChunkLoc;
        }
    }

    Noise updatingNoise;
    Vector2 setUpdatedLoc;

    private void UpdateChunk(Vector2 key, int x, int z, int pChunkX, int pChunkZ)
    {
        //Debug.Log("UpdateC");
        //Create new chunk
        setUpdatedLoc = new Vector2(-realChunkSize / 2 + x * realChunkSize, -realChunkSize / 2 + z * realChunkSize);
        chunks[key].chunkObj.transform.position = new Vector3(setUpdatedLoc.x, terrainFlipValue * z * chunkScale * ((chunkSize / 10) * 4) * terrainSlopeValue, setUpdatedLoc.y);
        chunks[key].isInArea = true;
        updatingNoise = chunks[key].chunkObj.GetComponent<Noise>();

        updatingNoise.planeWorldPos = new Vector2(x, z);
        //if (isFirstSpawn == false)
        //    noise.UpdateField();

        chunks[new Vector2(x, z)] = chunks[key];
        chunks.Remove(key);

        if (isFirstSpawn == false)
            updatingNoise.UpdateField();
    }

    void CreateChunks(int i)
    {
        GameObject chunk = Instantiate(noiseObject);

        Noise newNoise = chunk.GetComponent<Noise>();

        newNoise.planeSize = chunkSize + 1;
        newNoise.planeScale = chunkScale;
        newNoise.perlinFreq = perlinFreq;
        newNoise.textureMat = planeTexture;
        newNoise.physMat = physicsMat;
        newNoise.chunkFlipValue = terrainFlipValue;
        newNoise.heightScale = heightScale;
        newNoise.terSlopeVal = terrainSlopeValue;

        Chunk c = new Chunk();
        c.chunkObj = chunk;
        chunks.Add(new Vector2(1000 + i, 1000 + i), c);
    }

    //I don't know if you can apply this to the chunk that is at the end of a run but it would be cool
    Noise ApplyFlatNoiseMap(Vector2 key)
    {
        Noise newNoise = chunks[key].chunkObj.GetComponent<Noise>();

        newNoise.perlinFreq = 0.5f;
        newNoise.heightScale = 1;
        newNoise.terSlopeVal = 0.2f;

        return newNoise;
    }
}
