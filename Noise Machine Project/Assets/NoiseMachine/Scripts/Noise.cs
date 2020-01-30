using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise : MonoBehaviour
{
    #region Variables
    [Header("Objects & Scripts")]
    public GameObject noiseMarker;

    [Header("Create Noise Plane Variables")]
    //public bool randNoiseLoc;
    //public bool customRandNoise; // Use if you want to set custom random value
    //public bool randStartPoint;
    public int planeX;
    public int planeZ;
    public float planeScale;
    //public float heightScale;
    public float perlinScale;
    public Vector2 setRandomValue;

    [Header("Change Noise Plane Variables")]
    public bool autoUpdate;
    public float waveSpeed;
    public int wavePoints; //add this

    [Header("Output Variables")] // Do not edit in inspector
    public Vector2 randNum;
    public Vector2 startPoint;

    [Header("Hidden Variables")]
    private GameObject[,] markerObject;
    private float[,] noiseMap; // noiseMap will be an array of floats that matches the transform of markerobjects to produce a value
    private bool noiseFieldGenerated;
    private Vector2 randWaveGoal;
    #endregion

    //--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//

    // Make this into a function that can be repeated without starting function

    public float GetPerlinValue(int x, int z)
    {
        Vector2 fracCord = new Vector2((float)x / planeX, (float)z / planeZ);
        return Mathf.PerlinNoise(fracCord.x, fracCord.y);
    }

    private void Update()
    {
        if (Input.GetKeyDown("r"))
            GenerateNoiseField();

        if (autoUpdate == true)
            UpdateField();

        FieldWave();
    }

    public void GenerateNoiseField()
    {
        /*
        if (markerObject != null)
            for (int x = 0; x < planeX; x++)
                for (int z = 0; z < planeZ; z++)
                    Destroy(markerObject[x, z]);
        */

        markerObject = new GameObject[planeX, planeZ];

        /*
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
        */

        for (int x = 0; x < planeX; x++)
            for (int z = 0; z < planeZ; z++)
            {
                GameObject markerTmp = Instantiate(noiseMarker, new Vector3(x * planeScale, GetPerlinValue(x, z), z * planeScale), Quaternion.Euler(0, 0, 0));
                markerObject[x, z] = markerTmp;
            }

        noiseFieldGenerated = true;
    }

    public void FieldWave()
    {
        float step = Time.deltaTime * waveSpeed;

        if (randNum.x > randWaveGoal.x - 0.01f && randNum.x < randWaveGoal.x + 0.01f)
            randWaveGoal.y = Random.Range(-1.000f, 1.000f);

        if (randNum.y > randWaveGoal.y - 0.01f && randNum.y < randWaveGoal.y + 0.01f)
            randWaveGoal.y = Random.Range(-1.000f, 1.000f);
    }

    void UpdateField()
    {
        for (int x = 0; x < planeX; x++)
            for (int z = 0; z < planeZ; z++)
            {
                markerObject[x, z].transform.position = new Vector3(markerObject[x, z].transform.position.x, GetPerlinValue(x, z), markerObject[x, z].transform.position.z);
            }
    }
}