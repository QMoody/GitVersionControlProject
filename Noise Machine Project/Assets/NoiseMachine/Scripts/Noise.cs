using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise : MonoBehaviour
{
    #region Variables
    [Header("Objects & Scripts")]
    public GameObject noiseMarker;

    [Header("Create Noise Plane Variables")]
    public bool randNoiseLoc;
    public bool customRandNoise; // Use if you want to set custom random value
    public bool randStartPoint;
    public int planeX;
    public int planeZ;
    public float heightScale;
    public float noisePlaneScale; // 1 - normal scale / <1 - larger scale / >1 smaller scale
    public Vector2 setRandomValue;

    [Header("Change Noise Plane Variables")]
    public bool randWaveMov;
    public float waveSpeed;
    public int wavePoints; //add this

    [Header("Output Variables")] // Do not edit in inspector
    public Vector2 randNum;
    public Vector2 startPoint;

    [Header("Hidden Variables")]
    private GameObject[,] markerObject;
    private float[,] noiseMap; // noiseMap will be an array of floats that matches the transform of markerobjects to produce a value
    private bool noiseFieldGenerated;
    private Vector2 planeSetXY;
    private Vector2 randWaveGoal;
    private float startWaveGoal;
    #endregion

    //--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//

    // Make this into a function that can be repeated without starting function

    public bool autoUpdate;

    private void Update()
    {
        if (Input.GetKeyDown("r"))
            GenerateNoiseField();

        FieldWave();
    }

    public void GenerateNoiseField()
    {
        if (markerObject != null)
            for (int x = 0; x < planeX; x++)
                for (int z = 0; z < planeZ; z++)
                    Destroy(markerObject[x, z]);

        markerObject = new GameObject[planeX, planeZ];

        if (randNoiseLoc == true)
        {
            randNum.x = Random.Range(-1.000f, 1.000f);
            randNum.y = Random.Range(-1.000f, 1.000f);
        }
        else if (customRandNoise == true)
        {
            randNum.x = setRandomValue.x;
            randNum.y = setRandomValue.y;
        }
        else
        {
            randNum.x = 1;
            randNum.y = 1;
        }

        if (randStartPoint == true)
        {
            startPoint.x = Random.Range(-1.000f, 1.000f);
            startPoint.y = Random.Range(-1.000f, 1.000f);
        }
        else
        {
            startPoint.x = 0;
            startPoint.y = 0;
        }

        //noiseMap = new float[mapWidth?, mapHeight?];

        for (int x = 0; x < planeX; x++)
            for (int z = 0; z < planeZ; z++)
            {
                Vector2 fracCord = new Vector2((x + startPoint.x) / (planeX / noisePlaneScale) / randNum.x, (z + startPoint.y) / (planeZ / noisePlaneScale) / randNum.y); // Whole numbers return same Y value // Same values will always return same noise heights
                float noiseYValue = Mathf.PerlinNoise(fracCord.x, fracCord.y) * heightScale; // Height scale will change the noise intensity
                GameObject markerTmp = Instantiate(noiseMarker, new Vector3(x, noiseYValue, z), Quaternion.Euler(0, 0, 0));
                markerObject[x,z] = markerTmp;
                //noiseMap[x?, y?] = noiseYValue?;
            }

        MapDisplay display = FindObjectOfType<MapDisplay>();

        // the display will draw the noiseMap but it needs the values
        //display.DrawNoiseMap(noiseMap);
        planeSetXY = new Vector2(planeX, planeZ);
        
        randWaveGoal.x = Random.Range(-1.000f, 1.000f);
        randWaveGoal.y = Random.Range(-1.000f, 1.000f);
        noiseFieldGenerated = true;
    }

    public void FieldWave()
    {
        float step = Time.deltaTime * waveSpeed;

        if (randNum.x == randWaveGoal.x)
            randWaveGoal.y = Random.Range(-4.000f, 4.000f);

        if (randNum.y == randWaveGoal.y)
            randWaveGoal.y = Random.Range(-4.000f, 4.000f);

        if (noiseFieldGenerated == true)
        {
            if (randWaveMov == true)
            {
                randNum.x = Mathf.MoveTowards(randNum.x, randWaveGoal.x, step);
                randNum.y = Mathf.MoveTowards(randNum.y, randWaveGoal.y, step);

                for (int x = 0; x < planeX; x++)
                    for (int z = 0; z < planeZ; z++)
                    {
                        Vector2 fracCord = new Vector2((x + startPoint.x) / (planeX / noisePlaneScale) / randNum.x, (z + startPoint.y) / (planeZ / noisePlaneScale) / randNum.y);
                        float noiseYValue = Mathf.PerlinNoise(fracCord.x, fracCord.y) * heightScale;

                        markerObject[x, z].transform.position = new Vector3(markerObject[x, z].transform.position.x, noiseYValue, markerObject[x, z].transform.position.z);
                    }
            }
        }
    }
}