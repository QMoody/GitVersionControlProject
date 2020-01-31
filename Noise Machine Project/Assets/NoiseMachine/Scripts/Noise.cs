using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise : MonoBehaviour
{
    #region Variables
    [Header("Objects & Scripts")]
    public GameObject noiseMarker;

    [Header("Create Noise Plane Variables")]
    public int planeX;
    public int planeZ;
    public float planeScale;
    [Range(0.0f, 10.0f)] public float perlinFreq;
    public Vector2 setRandomValue;

    [Header("Change Noise Plane Variables")]
    public bool autoUpdate;

    //[Header("Output Variables")] // Do not edit in inspector

    [Header("Hidden Variables")]
    private GameObject[,] markerObject;
    public float[,] noiseMap; // noiseMap will be an array of floats that matches the transform of markerobjects to produce a value
    private bool noiseFieldGenerated;
    #endregion

    //--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//

    // Make this into a function that can be repeated without starting function

    public float GetPerlinValue(int x, int z)
    {
        Vector2 fracCord = new Vector2(perlinFreq * x / planeX, perlinFreq * z / planeZ);
        return Mathf.PerlinNoise(fracCord.x, fracCord.y);
    }

    private void Update()
    {
        if (Input.GetKeyDown("r"))
            GenerateNoiseField();

        if (autoUpdate == true && noiseFieldGenerated == true)
            UpdateField();
    }

    public void GenerateNoiseField()
    {
        /*
        if (markerObject != null)
            for (int x = 0; x < planeX; x++)
                for (int z = 0; z < planeZ; z++)
                    Destroy(markerObject[x, z]);
        */

        if (noiseFieldGenerated == false)
        {
            markerObject = new GameObject[planeX, planeZ];

            for (int x = 0; x < planeX; x++)
                for (int z = 0; z < planeZ; z++)
                {
                    GameObject markerTmp = Instantiate(noiseMarker, new Vector3(x * planeScale, GetPerlinValue(x, z), z * planeScale), Quaternion.Euler(0, 0, 0));
                    markerObject[x, z] = markerTmp;
                }

            noiseFieldGenerated = true;
        }
        else if (noiseFieldGenerated == true && autoUpdate == false)
        {
            UpdateField();
        }
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