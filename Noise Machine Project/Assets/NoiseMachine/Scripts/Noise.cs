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
    public float setRandomValue;

    [Header("Change Noise Plane Variables")]
    public bool randWaveMov;
    public float waveSpeed;
    public int wavePoints; //add this

    [Header("Output Variables")] // Do not edit in inspector
    public float randNum;
    public float startPoint;

    [Header("Hidden Variables")]
    private GameObject[,] markerObject;
    private bool noiseFieldGenerated;
    private Vector2 planeSetXY;
    private float randWaveGoal;
    private float startWaveGoal;
    #endregion

    //--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//

    // Make this into a function that can be repeated without starting function

    public bool autoUpdate;

    private void Start()
    {
        //FieldSetup();
    }

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
            randNum = Random.Range(-1.000f, 1.000f);
        else if (customRandNoise == true)
            randNum = setRandomValue;
        else
            randNum = 1;

        if (randStartPoint == true)
            startPoint = Random.Range(-1.000f, 1.000f);
        else
            startPoint = 0;

        for (int x = 0; x < planeX; x++)
            for (int z = 0; z < planeZ; z++)
            {
                Vector2 fracCord = new Vector2((x + startPoint) / (planeX / noisePlaneScale) / randNum, (z + startPoint) / (planeZ / noisePlaneScale) / randNum); // Whole numbers return same Y value // Same values will always return same noise heights
                float noiseYValue = Mathf.PerlinNoise(fracCord.x, fracCord.y) * heightScale; // Height scale will change the noise intensity
                GameObject markerTmp = Instantiate(noiseMarker, new Vector3(x, noiseYValue, z), Quaternion.Euler(0, 0, 0));
                markerObject[x,z] = markerTmp;
            }

        planeSetXY = new Vector2(planeX, planeZ);
        randWaveGoal = Random.Range(-1.000f, 1.000f);
        noiseFieldGenerated = true;
    }

    public void FieldWave()
    {
        float step = Time.deltaTime * waveSpeed;

        if (randNum == randWaveGoal)
            randWaveGoal = Random.Range(-1.000f, 1.000f);

        if (noiseFieldGenerated == true)
        {
            if (randWaveMov == true)
            {
                randNum = Mathf.MoveTowards(randNum, randWaveGoal, step);

                for (int x = 0; x < planeX; x++)
                    for (int z = 0; z < planeZ; z++)
                    {
                        Vector2 fracCord = new Vector2((x + startPoint) / (planeX / noisePlaneScale) / randNum, (z + startPoint) / (planeZ / noisePlaneScale) / randNum);
                        float noiseYValue = Mathf.PerlinNoise(fracCord.x, fracCord.y) * heightScale;

                        markerObject[x, z].transform.position = new Vector3(markerObject[x, z].transform.position.x, noiseYValue, markerObject[x, z].transform.position.z);
                    }
            }
        }
    }
}